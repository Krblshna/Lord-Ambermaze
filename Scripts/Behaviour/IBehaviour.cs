using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public interface IBehaviour
	{
        Vector2 MoveVector { get; }
        int MovesRemain { get; }
        void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker);
		void MakeMove();
		bool Wait();
		void OnMoveDone();
		void SpendMove(bool all);
		void RefreshMoveSteps();
        void OnChangeState(bool active);
    }
}