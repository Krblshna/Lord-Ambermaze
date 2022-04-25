using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.CellHighlight;
using LordAmbermaze.Core;
using LordAmbermaze.Core.Enums;
using UnityEngine;

namespace LordAmbermaze.Player.Skills
{
    public class BombSkill : MonoBehaviour, ISkill
    {
        [SerializeField] private bool tutor;
        [SerializeField] private GameObject _bombPrefab;
        [SerializeField] private GameObject _bombPreviewPrefab;
        [SerializeField] private Material _availableMaterial, _notAvailableMaterial;
        private SpriteRenderer _previewRenderer;
        private GameObject _bombPreview;
        private IBattleGround _battleGround;
        private IBattleManager _battleManager;
        private IPlayerInput _playerInput;
        private IAnimManager _animManager;
        private IBlocker _blocker;
        private Condition availableCondition = new Condition(StorageType.Counters, CommonCounters.BombSkill, 1);

        public SkillType skillType => SkillType.Bomb;
        private Vector2 AttackPosition => (Vector2)transform.position + _skillUseDirection;

        private Vector2Int _skillUseDirection = Vector2Int.right;
        private bool _active;
        private Func _callback;

        public void Init(Transform moveTransform)
        {
            _animManager = moveTransform.GetComponentInChildren<IAnimManager>();
            _battleGround = moveTransform.GetComponentInParent<IBattleGround>();
            _battleManager = moveTransform.GetComponentInParent<IBattleManager>();
            _blocker = moveTransform.GetComponentInParent<IBlocker>();
            _playerInput = moveTransform.GetComponentInChildren<IPlayerInput>();
            _bombPreview = Instantiate(_bombPreviewPrefab, transform);
            _previewRenderer = _bombPreview.GetComponentInChildren<SpriteRenderer>();
            _bombPreview.SetActive(false);
        }

        public void Update()
        {
            if (!_active) return;
            if (_playerInput.Left)
            {
                SetSkillDirection(Vector2Int.left);
            }
            else if (_playerInput.Right)
            {
                SetSkillDirection(Vector2Int.right);
            }
            else if (_playerInput.Up)
            {
                SetSkillDirection(Vector2Int.up);
            }
            else if (_playerInput.Down)
            {
                SetSkillDirection(Vector2Int.down);
            }
            if (_playerInput.Skip || _playerInput.ActivateSkill)
            {
                if (IsPossibleToCreate())
                    UseSkill();
            }

            if (_playerInput.Back || ButtonWrap.GetButtonDown(Buttons.Skill1))
            {
                Deactivate();
            }
        }

        private bool IsPossibleToCreate()
        {
            var tileData = _battleGround.GetTileData(AttackPosition);
            return tileData.slotType == SlotType.Empty;
        }

        private void UseSkill()
        {
            if (!tutor)
            {
                if (!PlayerState.TrySpendMana()) return;
            }

            var pushCollider = _battleGround.CheckComponent<ICharacterCollider>(AttackPosition);
            _animManager.Turn(_skillUseDirection);
            _animManager.Play(AnimTypes.kick, () =>
            {
                SoundManager.PlaySound(SoundType.place_bomb);
                var bomb = Instantiate(_bombPrefab, AttackPosition, Quaternion.identity, _battleGround.BattleTransform);
                var monster = bomb.GetComponent<IMonster>();
                _battleManager.Register(monster);
                //pushCollider?.Push(_skillUseDirection);
                _callback?.Invoke();
                Deactivate();
                bomb.GetComponent<IDamageable>().GetHit(1);
            });
        }

        private void SetSkillDirection(Vector2Int direction)
        {
            _skillUseDirection = direction;
            UpdateSkillArea();
        }

        private void UpdateSkillArea()
        {
            _bombPreview.transform.position = AttackPosition;
            _previewRenderer.material = IsPossibleToCreate() ? _availableMaterial : _notAvailableMaterial;
            _bombPreview.SetActive(true);
            //var instanceId = GetInstanceID();
            //CellHighlightManager.Instance.RemoveCellsColor(instanceId);
            //CellHighlightManager.Instance.SetCellsColor(instanceId, new Vector2[]{AttackPosition},
            //    CellHighlightType.Green);
        }

        public void Activate(Func callback)
        {
            _callback = callback;
            _blocker.Block(Blocks.SkillSelection);
            _active = true;
            UpdateSkillArea();
        }

        public bool IsAvailable()
        {
            return availableCondition.Test();
        }

        public void Deactivate()
        {
            _callback = null;
            _blocker.Unblock(Blocks.SkillSelection);
            _active = false;
            _bombPreview.SetActive(false);
            //CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
        }
    }
}