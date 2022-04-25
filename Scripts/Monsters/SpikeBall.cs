using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.InteractableSlots;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
	public class SpikeBall : Monster
	{
        protected override AInteractableSlots InteractableSlots { get; } = new SpikeBallInteractableSlots(); 
	}
}