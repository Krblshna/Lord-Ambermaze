using LordAmbermaze.Core;

namespace LordAmbermaze.Animations
{
	public class Move : Animation
	{
		public override AnimTypes AnimType => AnimTypes.move;
		public override string animName => AnimName.Move;
	}
}