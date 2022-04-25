using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using AZ.Core.UUID;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using LordAmbermaze.Monsters;
using LordAmbermaze.Sounds;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class BombAttacker : MonoBehaviour, IAttacker
    {
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private Group _attackGroup;
        private Transform _moveTransform;
		protected IEffect _effect;
        protected ISound _sound;
		private IDestructible destructible;
		private Func _onAttackMoment;

        private IBattleManager _battleManager;

        [SerializeField]
        private EMoveDirection[] _fireDirections;
        private IEnumerable<Vector2> _attackCells;

        private void Awake()
        {
            _attackCells = _fireDirections.Select(direction => Utils.DirectionToVector(direction));
        }

        public void Attack(Func onAttackMoment = null)
        {
            var opponentCollider = GetAttackedColliders();
            DoDamage(opponentCollider);
        }

        protected IEnumerable<ICharacterCollider> GetAttackedColliders()
        {
            var attackedColliders = new HashSet<ICharacterCollider>();
            foreach (var position in _attackCells)
            {
                var attackedCollider = _battleManager.BattleGround.GetTileCharCollider((Vector2)transform.position + position);
                if (attackedCollider != null)
                {
                    attackedColliders.Add(attackedCollider);
                }
            }

            return attackedColliders;
        }

        protected void DoDamage(IEnumerable<ICharacterCollider> attackedColliders)
        {
            foreach (var attackedCollider in attackedColliders)
            {
                attackedCollider?.Hit(_damageAmount, _attackGroup);
            }
        }

		public void Init(IBattleManager battleManager, Transform moveTransform)
		{
			_effect = GetComponent<IEffect>();
            _sound = GetComponent<ISound>();
			destructible = moveTransform.GetComponent<IDestructible>();
            _battleManager = battleManager;
			_moveTransform = moveTransform;
		}

		public void Attack(ICharacterCollider charCollider)
		{
			//DoDamage(charCollider);
			//Destruction();
		}

		public virtual void Attack(Vector2 attackDirection, Func onAttackMoment = null)
		{
            var opponentColliders = GetAttackedColliders();
            DoDamage(opponentColliders);
			Destruction();
		}

		void Destruction()
		{
			_effect?.Execute();
            _sound?.Play();
            destructible.MakeDestroy();
            _moveTransform.GetComponent<Monster_UUID>().SaveKill();
            _moveTransform.gameObject.SetActive(false);
		}

		//void DoDamage(ICharacterCollider attackedCollider)
		//{
		//	attackedCollider?.Hit(1);
		//	_onAttackMoment?.Invoke();
		//}
	}
}