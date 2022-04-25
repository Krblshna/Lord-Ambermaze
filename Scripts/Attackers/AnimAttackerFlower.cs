using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class AnimAttackerFlower : MonoBehaviour, IAttacker
    {
        private IAnimManager _animManager;
        private IBodyTiles _bodyTiles;
		private IBlocker _blocker;
		private IBattleManager _battleManager;

		private iTween.EaseType easeType = iTween.EaseType.linear;
        private Transform _moveTransform;
        private Func _onAttackMoment;

        private Dictionary<Vector2, Vector2[]> attackDeltaCells = new Dictionary<Vector2, Vector2[]>()
        {
            {
                Vector2.left, new Vector2[]
                {
                    new Vector2(-1, -2), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1)
                }
            },
            {
                Vector2.up, new Vector2[]
                {
                    new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1)
                }
            },
            {
                Vector2.right, new Vector2[]
                {
                    new Vector2(2, -2), new Vector2(2, -1), new Vector2(2, 0), new Vector2(2, 1)
                }
            },
            {
                Vector2.down, new Vector2[]
                {
                    new Vector2(-1, -2), new Vector2(0, -2), new Vector2(1, -2), new Vector2(2, -2)
                }
            }
        };

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
            _animManager.SetDirection(attackDirection);
            _animManager.Play(AnimTypes.attack, () =>
            {
                DoDamage(opponentCollider);
                _blocker.Unblock(Blocks.Attack);
                onAttackMoment?.Invoke();
            });
		}

        private IEnumerable<ICharacterCollider> GetAttackedColliders(Vector2 attackDirection)
        {
            var attackCells = GetCheckCells(attackDirection);
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

        private IEnumerable<Vector2> GetCheckCells(Vector2 attackDirection)
        {
            return attackDeltaCells[attackDirection].Select(attackDelta => attackDelta + (Vector2)_moveTransform.position);
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