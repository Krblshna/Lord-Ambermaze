
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface ICharacterCollider
	{
		void Init(IDamageable damageable, IMoveConnector battleUnitConnector, Group group);
		IMoveConnector GetMoveable();
		void Hit(int damage, Group group = Group.Enemy);
        void Push(Vector2Int pushDirection);
    }
}