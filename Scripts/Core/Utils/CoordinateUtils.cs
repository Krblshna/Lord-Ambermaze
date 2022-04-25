using UnityEngine;

namespace LordAmbermaze.Core
{
	public static class CoordinateUtils
	{
		static Vector2 _deltaVector = Vector2.one / 2;

		public static Vector2 PosToCoord(Vector2 position)
		{
			var v = position + _deltaVector;
			return new Vector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}

		public static Vector2 CoordToPos(Vector2 coordinate)
		{
			return coordinate - _deltaVector;
		}
	}
}