using System.Collections.Generic;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface ICanLongMove
	{
		void RequireMovement(List<Vector2> moveCells);
	}
}