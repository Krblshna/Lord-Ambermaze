using System;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public static class PlayerState
    {
        private static int initMaxHp = 2;
        private static int _currentHp = 2;
        public static int CurrentHp
        {
            get { return _currentHp; }
            private set
            {
                _currentHp = value;
            }
        }

        public static int CurrentMana { get; private set; } = 1;
		public static int MaxHp { get; private set; } = 2;
        public static int MaxMana { get; private set; } = 1;
        public static bool IsRight { get; set; }

		public static Vector2 PlayerLastPos { get; private set; }

        private static event Action OnHpChange;
        private static event Action OnManaChange;

        public static bool TrySpendMana(int manaCost = 1)
        {
            if (manaCost > CurrentMana) return false;
            CurrentMana -= manaCost;
            EventManager.TriggerEvent(EventList.ManaChanged);
            OnManaChange?.Invoke();
            return true;
        }

        public static void RestoreMana()
        {
            CurrentMana = MaxMana;
            EventManager.TriggerEvent(EventList.ManaChanged);
            OnManaChange?.Invoke();
        }

		public static void ChangeHp(int deltaHp)
		{
			var newHp = CurrentHp + deltaHp;
			CurrentHp = Mathf.Clamp(newHp, 0, MaxHp);
			OnHpChange?.Invoke();
		}

		public static void Kill()
        {
            ChangeHp(-CurrentHp);
        }

		public static void SubscribeHpChange(Action action)
		{
			OnHpChange += action;
		}

		public static void UnsubscribeHpChange(Action action)
		{
			OnHpChange -= action;
		}

        public static void SubscribeManaChange(Action action)
        {
            OnManaChange += action;
        }

        public static void UnsubscribeManaChange(Action action)
        {
            OnManaChange -= action;
        }

        public static void OnRestart()
        {
            ChangeHp(MaxHp);
            RestoreMana();
        }

        public static void SetLastPos(Vector2 pos)
        {
            PlayerLastPos = pos;
        }

        public static void UpdateMaxHp(int maxHp)
        {
            if (maxHp == 0) return;
            if (MaxHp == initMaxHp + maxHp) return;
            MaxHp = initMaxHp + maxHp;
            CurrentHp += 1;
			OnHpChange?.Invoke();
		}
    }
}