using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.TurnableObjects.Spikes
{
	public class SpikesCollider : MonoBehaviour
	{
		private int _damage = 2;

		void OnTriggerEnter2D(Collider2D collider)
		{
			var charCollider = collider.GetComponent<ICharacterCollider>();
			charCollider?.Hit(_damage, Group.Neutral);
		}
	}
}