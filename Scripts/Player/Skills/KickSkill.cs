using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.CellHighlight;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Player.Skills
{
    public class KickSkill : MonoBehaviour, ISkill
    {
        private IBattleGround _battleGround;
        private IPlayerInput _playerInput;
        private IAnimManager _animManager;
        private IBlocker _blocker;

        public SkillType skillType => SkillType.Kick;
        private Vector2 AttackPosition => (Vector2)transform.position + _skillUseDirection;

        private Vector2Int _skillUseDirection = Vector2Int.right;
        private bool _active;
        private Func _callback;

        public void Init(Transform moveTransform)
        {
            _animManager = moveTransform.GetComponentInChildren<IAnimManager>();
            _battleGround = moveTransform.GetComponentInParent<IBattleGround>();
            _blocker = moveTransform.GetComponentInParent<IBlocker>();
            _playerInput = moveTransform.GetComponentInChildren<IPlayerInput>();
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
            if (_playerInput.Skip)
            {
                UseSkill();
            }
        }

        private void UseSkill()
        {
            var pushCollider = _battleGround.CheckComponent<ICharacterCollider>(AttackPosition);
            _animManager.Turn(_skillUseDirection);
            _animManager.Play(AnimTypes.kick, () =>
            {
                pushCollider?.Push(_skillUseDirection);
                _callback?.Invoke();
                Deactivate();
            });
        }

        private void SetSkillDirection(Vector2Int direction)
        {
            _skillUseDirection = direction;
            UpdateSkillArea();
        }

        private void UpdateSkillArea()
        {
            var instanceId = GetInstanceID();
            CellHighlightManager.Instance.RemoveCellsColor(instanceId);
            CellHighlightManager.Instance.SetCellsColor(instanceId, new Vector2[]{AttackPosition},
                CellHighlightType.Green);
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
            throw new System.NotImplementedException();
        }

        public void Deactivate()
        {
            _callback = null;
            _blocker.Unblock(Blocks.SkillSelection);
            _active = false;
            CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
        }
    }
}