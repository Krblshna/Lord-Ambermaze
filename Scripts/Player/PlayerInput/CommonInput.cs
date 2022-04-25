using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public class CommonInput : Singleton<CommonInput>
	{
		public bool Left { get; private set; }
		public bool Right { get; private set; }
		public bool Up { get; private set; }
		public bool Down { get; private set; }

		public void Update()
		{
			Left = ButtonWrap.IsAxisDown(Direction.Left);
			Right = ButtonWrap.IsAxisDown(Direction.Right);
			Up = ButtonWrap.IsAxisDown(Direction.Up);
			Down = ButtonWrap.IsAxisDown(Direction.Down);
		}
	}
}