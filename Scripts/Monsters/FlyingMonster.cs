using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.InteractableSlots;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
	public class FlyingMonster : Monster
	{
        protected override AInteractableSlots InteractableSlots { get; } = new FlyingMonsterInteractableSlots(); 
	}
}