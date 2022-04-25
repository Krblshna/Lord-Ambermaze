using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class ProjectileBehaviour : MoveAttackBehaviour
	{
		public override void MakeMove()
		{
			if (MovesRemain < 1) return;
			_tileChecker.UpdateChecker(MoveVector.Direction());
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

		protected override void Attack(Func onAttack = null)
		{
			_characterEngine.UrgentAttack();
		}
	}
}