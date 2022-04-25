using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Level;
using LordAmbermaze.ScenesManagement;
using UnityEngine;

namespace LordAmbermaze.UI.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private TeleportationTransition _teleportationTransition;
        [SerializeField] private RectTransform mapRect;
        [SerializeField] private GameObject teleportHint;

        private Vector2Int _selectionPos, _playerPos;
        private Room[] _rooms;
        private Dictionary<Vector2Int, Room> _roomsDict;
        private bool _active, _initialized, _portal;

        private void Start()
        {
            EventManager.StartListening(EventList.TeleportEnter, OnTeleportEnter);
            EventManager.StartListening(EventList.TeleportExit, OnTeleportExit);
            EventManager.StartListening(EventList.SceneLoaded, SetMapPlayerPos);
        }
        public void Init()
        {
            if (_initialized) return;
            _initialized = true;
            _rooms = GetComponentsInChildren<Room>(true);
            _roomsDict = _rooms.ToDictionary(room => room.Position, room => room);
            SetMapPlayerPos();
        }

        public void OnOpened()
        {

        }

        private void OnTeleportEnter()
        {
            _portal = true;
        }

        private void OnTeleportExit()
        {
            _portal = false;
        }

        private void SetMapPlayerPos()
        {
            _playerPos = LevelManager.Instance.Pos;
            foreach (var room in _rooms)
            {
                room.SetPlayerPos(_playerPos);
            }
        }

        private void Update()
        {
            if (CommonBlocker.IsBlocked(CommonBlocks.Menu) 
                || CommonBlocker.IsBlocked(CommonBlocks.Battle) 
                || CommonBlocker.IsBlocked(CommonBlocks.Tutor)) return;
            if (ButtonWrap.GetButtonDown(Buttons.Map) || ButtonWrap.GetButtonDown(CommonButtons.Back))
            {
                if (_active)
                {
                    Deactivate();
                }
                else
                {
                    Activate();
                }
            }

            if (_active)
            {
                if (ButtonWrap.GetButtonDown(CommonButtons.Exit))
                {
                    Deactivate();
                }
                if (_portal)
                {
                    HandleInput();
                }
            }
        }
        public void Deactivate()
        {
            Time.timeScale = 1;
            _anim.SetBool("show", false);
            teleportHint.SetActive(false);
            DeselectAll();
            CommonBlocker.Unblock(CommonBlocks.Map);
            _active = false;
        }

        private void SetRectPos()
        {
            if (_portal)
            {
                mapRect.anchoredPosition = new Vector2(-320, 0);
                mapRect.localScale = Vector3.one * 0.9f;
            }
            else
            {
                mapRect.anchoredPosition = Vector2.zero;
                mapRect.localScale = Vector3.one;
            }
        }

        public void Activate()
        {
            SetRectPos();
            Init();
            Time.timeScale = 0;
            _anim.SetBool("show", true);
            _active = true;
            CommonBlocker.Block(CommonBlocks.Map);

            if (LevelManager.Instance.Pos != _playerPos)
            {
                SetMapPlayerPos();
            }

            if (_portal)
            {
                _selectionPos = _playerPos;
                teleportHint.SetActive(true);
                SelectItem();
            }
        }
        private void HandleInput()
        {
            if (ButtonWrap.IsAxisDown(Direction.Down))
            {
                MoveDown();
            }
            else if (ButtonWrap.IsAxisDown(Direction.Up))
            {
                MoveUp();
            }
            else if (ButtonWrap.IsAxisDown(Direction.Left))
            {
                MoveLeft();
            }
            else if (ButtonWrap.IsAxisDown(Direction.Right))
            {
                MoveRight();
            }

            if (ButtonWrap.GetButtonDown(Buttons.Use))
            {
                TryTeleport();
            }
        }
         
        private void TryTeleport()
        {
            if (!_portal) return;
            if (_selectionPos == _playerPos) return;
            if (_roomsDict.TryGetValue(_selectionPos, out var room))
            {
                if (room.MarkType != MarkType.Portal)
                {
                    room.WrongSelection();
                    SoundManager.PlaySound(SoundType.error);
                    return;
                }
                Teleport();
            }
        }

        private void Teleport() 
        {
            Deactivate();
            CommonBlocker.Block(CommonBlocks.Battle);
            Utils.SetTimeOut(() =>
            {
                CommonBlocker.Unblock(CommonBlocks.Battle);
                _teleportationTransition.Teleport(_selectionPos);
            }, 0.5f);
        }

        private void SelectItem(int deltaX = 0, int deltaY = 0)
        {
            var selectionPos = new Vector2Int(_selectionPos.x + deltaX, _selectionPos.y + deltaY);
            if (HaveRoom(selectionPos, out var room))
            {
                if (!room.Active) return;
                SoundManager.PlaySound(SoundType.room_switch);
                SelectRoom(room);
                _selectionPos = selectionPos;
            }
        }

        private void SelectRoom(Room selectedRoom)
        {
            foreach (var room in _rooms)
            {
                room.SetEnable(room.Position == selectedRoom.Position);
            }
        }

        private void DeselectAll()
        {
            foreach (var room in _rooms)
            {
                room.SetEnable(false);
            }
        }

        private bool HaveRoom(Vector2Int selectionPos, out Room room)
        {
            var haveRoom = _roomsDict.TryGetValue(selectionPos, out room);
            return haveRoom;
        }

        private void MoveDown()
        {
            SelectItem(0, -1);
        }

        private void MoveUp()
        {
            SelectItem(0, 1);
        }

        private void MoveRight()
        {
            SelectItem( 1);
        }

        private void MoveLeft()
        {
            SelectItem(-1);
        }
    }
}