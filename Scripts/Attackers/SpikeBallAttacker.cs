using System.Collections;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class SpikeBallAttacker : MonoBehaviour, IAttacker
    {
        private int _hitDamage = 100;
        private IAnimManager _animManager;
		private Transform _moveTransform;
		private float _time = 0.1f;
		private IBlocker _blocker;
		private IBattleManager _battleManager;
		private iTween.EaseType easeType = iTween.EaseType.linear;
		private Func moveToTarget;

		public void Init(IBattleManager battleManager, Transform moveTransform)
		{
			_moveTransform = moveTransform;
            _animManager = moveTransform.GetComponentInChildren<IAnimManager>();
			_battleManager = battleManager;
			_blocker = battleManager.Blocker;
		}

		public void Attack(ICharacterCollider charCollider)
		{
            DoDamage(charCollider);
		}

		public void Attack(Vector2 attackDirection, Func onAttackMoment = null)
        {
			var opponentCollider = GetAttackedCollider(attackDirection);
            _blocker.StartListeningUnblock(Blocks.Movement, () => DoDamage(opponentCollider));
            onAttackMoment?.Invoke();
		}

		private ICharacterCollider GetAttackedCollider(Vector2 attackDirection)
		{
			var position = (Vector2) _moveTransform.position + attackDirection;
			return _battleManager.BattleGround.GetTileCharCollider(position);
		}

		void DoDamage(ICharacterCollider attackedCollider)
		{
			attackedCollider?.Hit(_hitDamage);
			moveToTarget?.Invoke();
		}
	}
}