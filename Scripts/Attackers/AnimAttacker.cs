using System.Collections;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class AnimAttacker : MonoBehaviour, IAttacker
    {
        private IAnimManager _animManager;
		private Transform _moveTransform;
		private IBlocker _blocker;
		private IBattleManager _battleManager;
		private iTween.EaseType easeType = iTween.EaseType.linear;
		private Func _onAttackMoment;

		public void Init(IBattleManager battleManager, Transform moveTransform)
		{
			_moveTransform = moveTransform;
            _animManager = moveTransform.GetComponentInChildren<IAnimManager>();
			_battleManager = battleManager;
			_blocker = battleManager.Blocker;
		}

		public void Attack(ICharacterCollider charCollider)
		{
			
		}

		public void Attack(Vector2 attackDirection, Func onAttackMoment = null)
		{
            _blocker.Block(Blocks.Attack);
			var opponentCollider = GetAttackedCollider(attackDirection);
			_animManager.SetDirection(attackDirection);
			_animManager.Play(AnimTypes.attack, () =>
            {
                DoDamage(opponentCollider);
                _blocker.Unblock(Blocks.Attack);
                onAttackMoment?.Invoke();
			});
		}

		private ICharacterCollider GetAttackedCollider(Vector2 attackDirection)
		{
			var position = (Vector2) _moveTransform.position + attackDirection;
			return _battleManager.BattleGround.GetTileCharCollider(position);
		}

		void DoDamage(ICharacterCollider attackedCollider)
		{
			attackedCollider?.Hit(1);
			_onAttackMoment?.Invoke();
		}
	}
}