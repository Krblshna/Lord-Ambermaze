using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
    public class AreaAttacker : MonoBehaviour, IAreaAttacker
    {
        private IBattleManager _battleManager;

        [SerializeField]
        private EMoveDirection[] _fireDirections;
        private IEnumerable<Vector2> _attackCells;

        private void Awake()
        {
            _attackCells = _fireDirections.Select(direction => Utils.DirectionToVector(direction));
        }

        public void Init(Transform moveTransform)
        {
            _battleManager = moveTransform.GetComponentInParent<IBattleManager>();
        }
        
        public void Attack(Func onAttackMoment = null)
        {
            var opponentCollider = GetAttackedColliders();
            DoDamage(opponentCollider);
        }

        private IEnumerable<ICharacterCollider> GetAttackedColliders()
        {
            var attackedColliders = new HashSet<ICharacterCollider>();
            foreach (var position in _attackCells)
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