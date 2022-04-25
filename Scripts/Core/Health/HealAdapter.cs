using UnityEngine;

namespace LordAmbermaze.Core
{
	public class HealAdapter : MonoBehaviour, IHealAdapter
	{
		private IHealable _healable;

		private void Awake()
		{
			_healable = GetComponentInParent<IHealable>();
		}
		public void Heal(int amount)
		{
			_healable.Heal(amount);
		}
	}
}