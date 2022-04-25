using System.Collections;
using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class SimpleAttackerBig : MonoBehaviour, IAttacker
	{
		private IBodyTiles _bodyTiles;
		private IBlocker _blocker;
		private IBattleManager _battleManager;

		private float _time = 0.1f;
		private Transform _moveTransform;
		private iTween.EaseType easeType = iTween.EaseType.linear;

		public void Init(IBattleManager battleManager, Transform moveTransform)
		{
			_moveTransform = moveTransform;
			_battleManager = battleManager;
			_blocker = battleManager.Blocker;
			_bodyTiles = transform.parent.GetComponentInChildren<IBodyTiles>();
		}

		public void Attack(ICharacterCollider charCollider)
		{
			
		}

		public void Attack(Vector2 attackDirection, Func onAttackMoment = null)
		{
			StartCoroutine(AttackProcess(attackDirection));
		}

		IEnumerator AttackProcess(Vector2 attackDirection)
		{
			_blocker.Block(Blocks.Attack);
			var attackedColliders = GetAttackedColliders(attackDirection);
			Vector2 initPos = _moveTransform.position;
			var attackDelta = (Vector3)attackDirection / 2;
			iTween.MoveAdd(_moveTransform.gameObject, iTween.Hash(
				"amount", attackDelta,
				"time", _time,
				"easetype", easeType

			));

			yield return new WaitForSeconds(_time);
			DoDamage(attackedColliders);

			iTween.MoveTo(_moveTransform.gameObject, iTween.Hash(
				"position", (Vector3)initPos,
				"time", _time,
				"easetype", easeType

			));
			yield return new WaitForSeconds(_time);
			_blocker.Unblock(Blocks.Attack);
		}

		private IEnumerable<ICharacterCollider> GetAttackedColliders(Vector2 attackDirection)
		{
			var attackCells = _bodyTiles.GetCheckCells(attackDirection, _moveTransform.position);
			var attackedColliders = new List<ICharacterCollider>();
			foreach (var position in attackCells)
			{
				var attackedCollider = _battleManager.BattleGround.GetTileCharCollider(position);
				if (attackedCollider != null)
				{
					attackedColliders.Add(attackedCollider);
				}
			}

			return attackedColliders;
		}

		void DoDamage(IEnumerable<ICharacterCollider> attackedColliders)
		{
			foreach (var attackedCollider in attackedColliders)
			{
				attackedCollider?.Hit(1);
			}
		}
	}
}