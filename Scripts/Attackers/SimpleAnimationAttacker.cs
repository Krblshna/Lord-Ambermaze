using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class SimpleAnimationAttacker : MonoBehaviour, IAttacker
	{
		private Transform _moveTransform;
		private float _time = 0.1f;
		private IBlocker _blocker;
		private IBattleManager _battleManager;
		private iTween.EaseType easeType = iTween.EaseType.linear;
		private Func _onAttackMoment;

		public void Init(IBattleManager battleManager, Transform moveTransform)
		{
			_moveTransform = moveTransform;
			_battleManager = battleManager;
			_blocker = battleManager.Blocker;
		}

		public void Attack(ICharacterCollider charCollider)
		{

		}

		public void Attack(Vector2 attackDirection, Func onAttackMoment = null)
		{
			var opponentCollider = GetAttackedCollider(attackDirection);
			DoDamage(opponentCollider);
		}

		private ICharacterCollider GetAttackedCollider(Vector2 attackDirection)
		{
			var position = (Vector2)_moveTransform.position + attackDirection;
			return _battleManager.BattleGround.GetTileCharCollider(position);
		}

		void DoDamage(ICharacterCollider attackedCollider)
		{
			attackedCollider?.Hit(1);
			_onAttackMoment?.Invoke();
		}
	}
}