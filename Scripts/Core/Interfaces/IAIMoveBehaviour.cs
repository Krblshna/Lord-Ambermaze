using System.Collections.Generic;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface IAIMoveBehaviour
	{
		Vector2 GetMoveVector();
		void Wait();
		void OnMoveDone();

		void RecalcMoveDirection();
		void SetDirection(Vector2 direction);
		void Init(Transform moveTransform, IEnumerable<SlotType> obstacleSlots);
	}
}