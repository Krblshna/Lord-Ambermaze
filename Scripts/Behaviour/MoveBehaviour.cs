using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class MoveBehaviour : MoveAttackBehaviour
	{
		public override void MakeMove()
		{
			if (MovesRemain < 1) return;
			_moveBehaviour.RecalcMoveDirection();
			var direction = MoveVector.Direction();
			_animManager.Turn(direction);
			_tileChecker.UpdateChecker(direction);
			if (_tileChecker.CouldMove())
            {
                Move();
            }
			else if (_tileChecker.ShouldConnect())
			{
				Connect();
			}
			else if (HaveMaxMoves())
			{
				MetObstacle();
			}
			else
			{
				_characterEngine.SpendMove(true);
			}
		}
    }
}