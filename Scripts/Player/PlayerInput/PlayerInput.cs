using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public class PlayerInput : MonoBehaviour, IPlayerInput
	{
		[SerializeField] private bool _autoSkip;
		public bool Left { get; protected set; }
		public bool Right { get; protected set; }
		public bool Up { get; protected set; }
		public bool Down { get; protected set; }
		public bool Skip { get; protected set; }
        public bool Back { get; protected set; }
		public bool UsedSkill { get; protected set; }
        public bool ActivateSkill { get; protected set; }
		public bool Skill1 { get; protected set; }
        public bool Skill2 { get; protected set; }
        public bool Skill3 { get; protected set; }
        public bool Skill4 { get; protected set; }

		private HashSet<EMoveDirection> directionUsed = new HashSet<EMoveDirection>();

		public void CustomUpdate()
		{
			HandleInput();
		}

		//public void CustomUpdateOld()
		//{
		//	Left = ButtonWrap.IsAxisDown(Direction.Left);
		//	Right = ButtonWrap.IsAxisDown(Direction.Right);
		//	Up = ButtonWrap.IsAxisDown(Direction.Up);
		//	Down = ButtonWrap.IsAxisDown(Direction.Down);
		//	Skip = InputManagerC.GetButtonDown(Buttons.Use);
		//}

		private bool checkCondition(float axis, bool positive)
		{
			if (positive)
			{
				return axis > Utils.axis_min_value;
			}
			return axis < -Utils.axis_min_value;
		}

		private bool HandleDirection(float axis, bool positive, EMoveDirection eMoveDirection)
        {
            //return checkCondition(axis, positive);
			if (checkCondition(axis, positive))
			{
				if (!directionUsed.Contains(eMoveDirection))
				{
					directionUsed.Add(eMoveDirection);
					return true;
				}
			}
			else
			{
				directionUsed.Remove(eMoveDirection);
			}
			return false;
		}

		public virtual void HandleInput()
        {
            if (CommonBlocker.IsBlocked(CommonBlocks.Menu)) return;
			var horizontalAxis = InputManagerC.GetAxis(InputAxis.MoveHorizontal);
			Right = HandleDirection(horizontalAxis, true, EMoveDirection.Right);
			Left = HandleDirection(horizontalAxis, false, EMoveDirection.Left);

			var verticalAxis = InputManagerC.GetAxis(InputAxis.MoveVertical);
			Up = HandleDirection(verticalAxis, true, EMoveDirection.Up);
			Down = HandleDirection(verticalAxis, false, EMoveDirection.Down);

			Skip = _autoSkip || ButtonWrap.GetButtonDown(CommonButtons.Use);
            Back = InputManagerC.GetButtonDown(CommonButtons.Back) || InputManagerC.GetButtonDown(CommonButtons.Exit);
			Skill1 = ButtonWrap.GetButtonDown(Buttons.Skill1);
            Skill2 = InputManagerC.GetButtonDown(Buttons.Skill2);
            Skill3 = InputManagerC.GetButtonDown(Buttons.Skill3);
            Skill4 = InputManagerC.GetButtonDown(Buttons.Skill4);
			UsedSkill = Skill1 || Skill2 || Skill3 || Skill4;
        }
	}
}