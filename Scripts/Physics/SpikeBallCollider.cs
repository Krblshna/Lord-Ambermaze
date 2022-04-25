using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Physics
{
    public class SpikeBallCollider : MonoBehaviour
    {
        private IAttacker _attacker;
        private void Awake()
        {
            _attacker = transform.parent.GetComponentInChildren<IAttacker>();
        }
		void OnTriggerEnter2D(Collider2D collider)
        {
            var charCollider = collider.GetComponent<ICharacterCollider>();
            if (collider.tag != Tags.Wall)
            {
                if (charCollider == null) return;
            }

            _attacker.Attack(charCollider);
        }
	}
}