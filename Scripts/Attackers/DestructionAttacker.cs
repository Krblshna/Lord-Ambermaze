using System.Collections;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class DestructionAttacker : MonoBehaviour, IAttacker
	{
		private Transform _moveTransform;
		private float _time = 0.1f;
		private IBlocker _blocker;
		private IBattleManager _battleManager;
		private IEffect destroyEffect;
		private IDestructible destructible;
		private iTween.EaseType easeType = iTween.EaseType.linear;
		private Func _onAttackMoment;

		public void Init(IBattleManager battleManager, Transform moveTransform)
		{
			destroyEffect = GetComponent<IEffect>();
			destructible = moveTransform.GetComponent<IDestructible>();

			_moveTransform = moveTransform;
			_battleManager = battleManager;
			_blocker = battleManager.Blocker;
		}

		public void Attack(ICharacterCollider charCollider)
		{
			DoDamage(charCollider);
			Destruction();
			//_blocker.Unblock(Blocks.Movement);
			//_blocker.Unblock(Blocks.IntermediateMovement);
		}

		public void Attack(Vector2 attackDirection, Func onAttackMoment = null)
		{
			_onAttackMoment = onAttackMoment;
			StartCoroutine(AttackProcess(attackDirection));
		}

		IEnumerator AttackProcess(Vector2 attackDirection)
		{
			_blocker.Block(Blocks.Movement);
			var opponentCollider = GetAttackedCollider(attackDirection);
			Vector2 initPos = _moveTransform.position;
			var attackDelta = (Vector3)attackDirection / 2;
			iTween.MoveAdd(_moveTransform.gameObject, iTween.Hash(
				"amount", attackDelta,
				"time", _time,
				"easetype", easeType

			));
			yield return new WaitForSeconds(_time);
			//yield return new WaitForSeconds(_time);
			_blocker.Unblock(Blocks.Movement);
			Attack(opponentCollider);
		}

		private ICharacterCollider GetAttackedCollider(Vector2 attackDirection)
		{
			var position = (Vector2)_moveTransform.position + attackDirection;
			return _battleManager.BattleGround.GetTileCharCollider(position);
		}

		void Destruction()
		{
			destroyEffect?.Execute();
			destructible.MakeDestroy();
			_moveTransform.gameObject.SetActive(false);
		}

		void DoDamage(ICharacterCollider attackedCollider)
		{
			attackedCollider?.Hit(1);
			_onAttackMoment?.Invoke();
		}
	}
}