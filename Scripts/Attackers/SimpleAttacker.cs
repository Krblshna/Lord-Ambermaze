using System.Collections;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class SimpleAttacker : MonoBehaviour, IAttacker, IHaveOnDeathEvent
	{
		private Transform _moveTransform;
		private float _time = 0.1f;
		private IBlocker _blocker;
		private IBattleManager _battleManager;
		private iTween.EaseType easeType = iTween.EaseType.linear;
		private Func _onAttackMoment;
        private IEnumerator _attackProcess;

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
			_onAttackMoment = onAttackMoment;
            _attackProcess = AttackProcess(attackDirection);
			StartCoroutine(_attackProcess);
		}

		IEnumerator AttackProcess(Vector2 attackDirection)
		{
			_blocker.Block(Blocks.Attack);
			var opponentCollider = GetAttackedCollider(attackDirection);
			Vector2 initPos = _moveTransform.position;
			var attackDelta = (Vector3)attackDirection / 2;
			iTween.MoveAdd(_moveTransform.gameObject, iTween.Hash(
				"amount", attackDelta,
				"time", _time,
				"easetype", easeType

			));

			yield return new WaitForSeconds(_time);
			DoDamage(opponentCollider);

			iTween.MoveTo(_moveTransform.gameObject, iTween.Hash(
				"position", (Vector3)initPos,
				"time", _time,
				"easetype", easeType

			));
			yield return new WaitForSeconds(_time);
			_blocker.Unblock(Blocks.Attack);
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

        public void OnDeathEvent()
        {
            if (_attackProcess != null)
            {
				StopCoroutine(_attackProcess);
				_blocker.Unblock(Blocks.Attack);
			}
        }
    }
}