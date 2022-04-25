using System.Collections;
using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class AnimAttackerBig : MonoBehaviour, IAttacker
    {
        private IAnimManager _animManager;
        private IBodyTiles _bodyTiles;
		private IBlocker _blocker;
		private IBattleManager _battleManager;

		private iTween.EaseType easeType = iTween.EaseType.linear;
        private Transform _moveTransform;
        private Func _onAttackMoment;

		public void Init(IBattleManager battleManager, Transform moveTransform)
		{
			_moveTransform = moveTransform;
            _animManager = moveTransform.GetComponentInChildren<IAnimManager>();
			_battleManager = battleManager;
			_blocker = battleManager.Blocker;
            _bodyTiles = transform.parent.GetComponentInChildren<IBodyTiles>();
        }

		public void Attack(ICharacterCollider charCollider)
		{
			
		}

		public void Attack(Vector2 attackDirection, Func onAttackMoment = null)
		{
            _blocker.Block(Blocks.Attack);
            var opponentCollider = GetAttackedColliders(attackDirection);
			_animManager.Play(AnimTypes.attack, () =>
            {
                DoDamage(opponentCollider);
                _blocker.Unblock(Blocks.Attack);
			});
		}

        private IEnumerable<ICharacterCollider> GetAttackedColliders(Vector2 attackDirection)
        {
            var attackCells = _bodyTiles.GetCheckCells(attackDirection, _moveTransform.position);
            var attackedColliders = new HashSet<ICharacterCollider>();
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