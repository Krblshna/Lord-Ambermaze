using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Consumables
{
	public class FirstAidKit : ActiveObject
	{
		protected override void Activate(Collider2D collider)
		{
			var healAdapter = collider.GetComponent<IHealAdapter>();
			if (healAdapter == null) return;
			healAdapter.Heal(1);
			SoundManager.PlaySound(SoundType.collect_apple);
			Destroy(gameObject);
		}
	}
}