using System.Collections.Generic;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface IMoveBehaviour
	{
		Vector2 GetMoveVector();
        int GetMaxMoves();
		void Wait();
		void OnMoveDone();

		void RecalcMoveDirection();
		void SetDirection(Vector2 direction);
		void Init(Transform moveTransform, IEnumerable<SlotType> obstacleSlots);
        void OnChangeState(bool active);
    }
}