using AZ.Core;
using AZ.Core.UUID;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Consumables
{
	public class ActiveObject : UUID
	{
		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.CompareTag(Tags.Player) && !CommonBlocker.IsBlocked(CommonBlocks.Battle))
			{
				Activate(collider);
			}
		}

		protected virtual void Activate(Collider2D collider) { }
	}
}