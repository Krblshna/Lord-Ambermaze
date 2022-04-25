using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public class Health : MonoBehaviour, IHealth
	{
		private IHealthUI _healthUI;

		[SerializeField] private int _maxHealth = 2;
        

        public int CurrentHealth { get; private set; }
		public int MaxHealth { get; private set; }
        public event Func OnChangeHp;

		public void Init(Transform parentTransform)
		{
			_healthUI = parentTransform.GetComponentInChildren<IHealthUI>();
			MaxHealth = _maxHealth;
			CurrentHealth = MaxHealth;
            if (_healthUI == null) return;
            _healthUI.Init(MaxHealth);
            _healthUI.UpdateHP(CurrentHealth);
        }

        public void Kill()
        {
            _healthUI?.UpdateHP(0);
            OnChangeHp?.Invoke();
        }

		public int ChangeHealth(int amount)
		{
			CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            _healthUI?.UpdateHP(CurrentHealth);
            OnChangeHp?.Invoke();
			return CurrentHealth;
		}

        public void Hide()
        {
            _healthUI?.Hide();

        }
    }
}