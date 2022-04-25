using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface IDamageHandler
	{
		void GetHit();
		void Heal();
		void Death(Func callback);
        void Init(Transform transform);
    }
}