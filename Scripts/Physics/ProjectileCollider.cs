using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Physics
{
	public class ProjectileCollider : MonoBehaviour, IAttackCollider, IHaveOnDeathEvent
	{
		private IAttacker _attacker;
		private IMover _mover;
        private bool _dead = false;

		public void Init(Transform moveTransform)
		{
			_attacker = moveTransform.GetComponentInChildren<IAttacker>();
			_mover = moveTransform.GetComponentInChildren<IMover>();
		}

		void OnTriggerEnter2D(Collider2D collider)
        {
            if (_dead) return;
			var charCollider = collider.GetComponent<ICharacterCollider>();
			if (collider.tag != Tags.Wall)
			{
				if (charCollider == null) return;
			}

			_attacker.Attack(charCollider);
			_mover.Stop();
		}

        public void OnDeathEvent()
        {
            _dead = true;
        }
    }
}