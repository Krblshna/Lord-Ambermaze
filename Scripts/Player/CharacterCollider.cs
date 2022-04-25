using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using UnityEngine;

namespace LordAmbermaze.Player
{
    public class CharacterCollider : MonoBehaviour, ICharacterCollider
	{
        [SerializeField] private bool _flying;
	    private IMoveConnector _battleUnitConnector;
	    private IDamageable _damageable;
        private IPushable _pushable;
        private Group _group;

		public void Init(IDamageable damageable, IMoveConnector battleUnitConnector, Group group)
        {
	        _damageable = damageable;
	        _battleUnitConnector = battleUnitConnector;
            _group = group;
			_pushable = GetComponentInParent<IPushable>();
		}

	    public IMoveConnector GetMoveable()
	    {
		    return _battleUnitConnector;
	    }

        public void Push(Vector2Int pushDirection)
        {
            _pushable.Push(pushDirection);
        }

		public void Hit(int damage, Group group = Group.Enemy)
        {
            if (IsFriendlyFire(group)) return;
			_damageable.GetHit(damage);
        }

        private bool IsFriendlyFire(Group group)
        {
            return _group == group;
        }

		//void OnTriggerEnter2D(Collider2D collider)
	 //   {
		//    if (collider.tag == Tags.Lake && !_flying)
		//    {
		//	    Hit(100);
		//    }
	 //   }
	}
}
