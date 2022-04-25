using System.Collections.Generic;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface IBodyTiles
	{
		List<Vector2> GetCheckCells(Vector2 moveVector);
		List<Vector2> GetCheckCells(Vector2 moveVector, Vector2 initPos);
		List<Vector2> GetMoveCells(Vector2 moveVector);
        Vector2[,] Tiles { get; }
	}
}