using System;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Level;
using UnityEngine;
using UnityEngine.UI;

namespace LordAmbermaze.UI.Map
{
    [System.Serializable]
    public class RoomGateUI
    {
        public GameObject Gate;
        private Color _activeColor = Color.white, _inactiveColor = Color.red;
        private Image image;
        public RoomLink RoomLink;
        private Condition _openCondition;

        public void Init(Color inactiveColor)
        {
            image = Gate.GetComponent<Image>();
            _inactiveColor = inactiveColor;
        }

        public void SetActive(bool active)
        {
            Gate.SetActive(active);
        }

        public void UpdateLocked()
        {
            if (image == null) return;
            image.color = IsOpened() ? _activeColor : _inactiveColor;
        }

        public void SetCondition(string roomLockStr)
        {
            _openCondition = new Condition(StorageType.Counters, roomLockStr, 1);
            //EventManager.StartListening("counters:" + roomLockStr, UpdateLocked);
            UpdateLocked();
        }

        public bool IsOpened()
        {
            if (_openCondition == null) return true;
            return _openCondition.Test();
        }
    }

    [System.Serializable]
    public class RoomMarkUI
    {
        public GameObject Mark;
        public MarkType MarkType;

        public void SetActive(MarkType markType)
        {
            Mark.SetActive(MarkType == markType);
        }
    }

    public class Room : MonoBehaviour
    {
        [SerializeField] private Color _inactiveColor = Color.red;
        [SerializeField] private GameObject _activeBorder, _playerMark, _container;
        [SerializeField] private RoomGateUI[] _gates;
        [SerializeField] private RoomMarkUI[] _marks;
        private Animator _anim;
        public bool Active => _container.activeSelf;
        private Vector2Int _pos;
        private RoomData _roomData;
        private MarkType _markType;
        private RectTransform rect;
        private Vector2 _startPos = new Vector2(-91, -27);
        private readonly Vector2 _roomSize = new Vector2(98, 58);
        private Condition _openCondition;
        private Reward _openReward;
        private RoomLock[] _roomLocks;

        public Vector2Int Position => _pos;
        public MarkType MarkType => _markType;
        public void SetEnable(bool active)
        {
            _activeBorder.SetActive(active);
        }

        public void Init(RoomData roomData, MarkType markType, Vector2 startPos)
        {
            _startPos = startPos;
            var curLevel = LevelManager.Instance.CurLevel;
            var curBiom = LevelManager.Instance.CurBiom;
            _roomData = roomData;
            _markType = markType;
            _pos = _roomData.Pos;
            var roomStr = $"biom-{curBiom}level-{curLevel}-pos-{_pos}";
            _openCondition = new Condition(StorageType.Counters, roomStr, 1);
            _openReward = new Reward(StorageType.Counters, roomStr, 1);
            CheckOpen();
            //EventManager.StartListening($"counters:{roomStr}", OnOpen);
            rect = GetComponent<RectTransform>();
            _anim = GetComponent<Animator>();
            _roomLocks = roomData.RoomLocks;
            foreach (var gate in _gates)
            {
                gate.Init(_inactiveColor);
            }
            foreach (var roomLock in _roomLocks)
            {
                var gate = _gates.First(gateData => gateData.RoomLink == roomLock.Direction);
                gate.SetCondition(roomLock.LockStr);
            }
            rect.anchoredPosition = _startPos + new Vector2(_pos.x * _roomSize.x, _pos.y * _roomSize.y);
            SetGates();
            SetMark();
        }

        public void CheckOpen()
        {
            _container.SetActive(Condition.Test(_openCondition));
            foreach (var gate in _gates)
            {
                gate.UpdateLocked();
            }
        }

        private void SetMark()
        {
            foreach (var mark in _marks)
            {
                mark.SetActive(_markType);
            }
        }

        private void SetGates()
        {
            foreach (var gate in _gates)
            {
                var active = _roomData.RoomsLinks.Contains(gate.RoomLink);
                gate.SetActive(active);
            }
        }

        public void SetPlayerPos(Vector2Int playerPos)
        {
            var active = _pos == playerPos;
            foreach (var gate in _gates)
            {
                gate.UpdateLocked();
            }
            if (active && !Condition.Test(_openCondition))
            {
                //_openReward.Claim();
                //_container.SetActive(true);
            }
            CheckOpen();
            _playerMark.SetActive(active);
        }

        public void WrongSelection()
        {
            _anim.SetTrigger("active");
        }
    }
}