using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface IHealth
	{
		void Init(Transform parentTransform);
		int ChangeHealth(int amount);
        void Kill();
		int CurrentHealth { get; }
        int MaxHealth { get; }
        event Func OnChangeHp;
        void Hide();
    }
}