using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.UI;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public class PlayerHealth : MonoBehaviour, IHealth
    {
        public int MaxHealth => PlayerState.MaxHp;
        public event Func OnChangeHp;
        public void Hide()
        {
        }

        public int CurrentHealth => PlayerState.CurrentHp;
		private HpSystem _hpSystem;
		private HpSystem HpSystem
		{
			get
			{
				if (_hpSystem == null)
				{
					_hpSystem = FindObjectOfType<HpSystem>();
				}
				return _hpSystem;
			}
		}
		public void Init(Transform parentTransform)
		{
            _hpSystem = FindObjectOfType<HpSystem>();
            if (_hpSystem != null)
            {
				_hpSystem.UpdateHearts();
            }
			
		}
        public void Kill()
        {
            PlayerState.Kill();
            HpSystem.UpdateHearts();
		}

		public int ChangeHealth(int amount)
		{
			PlayerState.ChangeHp(amount);
			HpSystem.UpdateHearts();
			return CurrentHealth;
		}
	}
}