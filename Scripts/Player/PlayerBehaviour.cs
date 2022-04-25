using System.Numerics;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.InteractableSlots;
using LordAmbermaze.Interactions;
using LordAmbermaze.Movers;
using LordAmbermaze.Player.Skills;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace LordAmbermaze.Player
{
	public class PlayerBehaviour : MonoBehaviour
	{
		private IPlayerInput _playerInput;
		private IMover _mover;
		private IAttacker _attacker;
		private IBattleManager _battleManager;
		private IBattleGround _battleGround;
		protected IBlocker _blocker;
		private IAnimManager _animManager;
        private ICellEffector _cellEffector;
        private IInteractionsManager _interactionsManager;
        private ISkillsManager _skillsManager;

        private Transform _moveTransform;
		private bool active;

        private TileData _tileData;
        private TileData _groundTileData; 

		public void Init(IBattleManager battleManager, Transform moveTransform, AInteractableSlots interactableSlots)
		{
			_playerInput = GetComponent<IPlayerInput>();
			_mover = GetComponent<IMover>();
			_attacker = GetComponent<IAttacker>();
            _interactionsManager = moveTransform.GetComponentInChildren<IInteractionsManager>();
            _skillsManager = moveTransform.GetComponentInChildren<ISkillsManager>();
			_animManager = moveTransform.GetComponentInChildren<IAnimManager>();
            _cellEffector = moveTransform.GetComponentInChildren<ICellEffector>();
			var autoMover = GetComponentInChildren<IAutoMover>();
            var dungeonMover = GetComponentInChildren<IDungeonAutoMover>();

			_skillsManager.Init(moveTransform);
			_interactionsManager.Init(moveTransform);
			_mover.Init(moveTransform);
			_attacker.Init(battleManager, moveTransform);
			autoMover.Init(moveTransform, _animManager);
            dungeonMover.Init(moveTransform, _animManager);

			_moveTransform = moveTransform;
			_battleManager = battleManager;
			_battleGround = battleManager.BattleGround;
			_blocker = _battleManager.Blocker;
		}

		private void LateUpdate()
		{
			_playerInput.CustomUpdate();
			HandleMovement();
		}

		protected virtual bool IsBlocked()
		{
			return CommonBlocker.IsBlocked(CommonBlocks.Loading) 
                   || _blocker.IsBlocked(Blocks.Movement) 
                   || _blocker.IsBlocked(Blocks.IntermediateMovement)
                   || CommonBlocker.IsBlocked(CommonBlocks.Map)
                   || CommonBlocker.IsBlocked(CommonBlocks.Tutor)
                   || CommonBlocker.IsBlocked(CommonBlocks.Battle)
				   || _blocker.IsBlocked(Blocks.Attack)
                   || _blocker.IsBlocked(Blocks.SkillSelection);
		}

		private void HandleMovement()
		{
			if (IsBlocked()) return;
            if (_playerInput.UsedSkill)
            {
                TryUseSkill();
            }
			if (_playerInput.Left)
			{
				TryMove(Vector2.left);
			} else if (_playerInput.Right)
			{
				TryMove(Vector2.right);
			}
			else if (_playerInput.Up)
			{
				TryMove(Vector2.up);
			}
			else if (_playerInput.Down)
			{
				TryMove(Vector2.down);
			}

			if (_playerInput.Skip)
			{
				Wait();
			}
		}

        private int GetActiveSkill()
        {
            if (_playerInput.Skill1) return 1;
            if (_playerInput.Skill2) return 2;
            if (_playerInput.Skill3) return 3;
            if (_playerInput.Skill4) return 4;
            return 0;
        }

        private void TryUseSkill()
        {
            var activeSkill = GetActiveSkill();
            if (_skillsManager.HaveAvailableSkill(activeSkill))
            {
                _skillsManager.ActivateSkill(activeSkill, Wait);
            }
        }

        private bool CouldMove()
        {
            var slotType = _tileData.slotType;
			return slotType == SlotType.Empty || slotType == SlotType.Projectile || slotType == SlotType.Lake || slotType == SlotType.Pit;
        }

        private bool CouldAttack()
        {
            var slotType = _tileData.slotType;
            var groundSlotType = _groundTileData.slotType;
            return slotType == SlotType.Enemy && groundSlotType != SlotType.Wall;
        }

        private bool CouldInteract(Vector2 newPos, out IInteractable interactable)
        {
            interactable = _battleGround.CheckComponent<IInteractable>(newPos);
            return interactable != null;
        }

		private void TryMove(Vector2 direction)
		{
			_animManager.Turn(direction);
			var newPos = _moveTransform.position + (Vector3) direction;
            _tileData = _battleGround.GetTileData(newPos);
            _groundTileData = _battleGround.GetGroundTileData(newPos);
			if (CouldMove())
			{
				Move(newPos);
			}
			else if (CouldInteract(newPos, out var interactable))
            {
                Interact(interactable);
            }
			else if (CouldAttack())
			{
				Attack(direction);
			}
			else
			{
				Wait();
			}
		}

        private void Interact(IInteractable interactable)
        {
            if (interactable.Available)
            {
                _interactionsManager.Interact(interactable, Wait);
            }
            else
            {
				Wait();
            }
        }

		private void Move(Vector2 newPos)
		{
			_blocker.Block(Blocks.Movement);
			_animManager.Play(AnimTypes.move);
			_mover.MoveTo(newPos, OnMoveFinish);
			PlayerState.SetLastPos(_moveTransform.position);
			_battleManager.PlayerNextPos(newPos);
		}

		private void Attack(Vector2 direction)
		{
			var pos = _moveTransform.position;
			_blocker.Block(Blocks.Attack);
            var animType = AnimTypeUtils.GetAttackAnimType(direction);
			SoundManager.PlaySound(SoundType.sword_swing_1);
			_animManager.Play(animType, () =>
			{
				_attacker.Attack(direction);
				_battleManager.PlayerNextPos(pos);
				_blocker.Unblock(Blocks.Attack);
			});
		}

		private void Wait()
		{
			_battleManager.PlayerNextPos(_moveTransform.position);
		}

		private void OnMoveFinish()
		{
			//_animManager.Play(AnimTypes.idle);
			_blocker.Unblock(Blocks.Movement);
            _cellEffector.Check();
		}

        public void Death()
        {
            _mover.Stop();
        }
    }
}