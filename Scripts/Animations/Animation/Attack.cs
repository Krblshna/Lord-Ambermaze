using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Animations
{
	public class Attack : Animation
	{
		public override AnimTypes AnimType => AnimTypes.attack;
		public override string animName => AnimName.Attack;
	}
}