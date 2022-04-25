using System.Collections.Generic;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
	public class ColliderBodyTile : MonoBehaviour, IBodyTiles
	{
        public Vector2[,] Tiles => _tiles;

		private BoxCollider2D _collider;
		private Vector2[,] _tiles;
		private int _sizeX, _sizeY;

        private void Start()
		{
			InitTiles();
		}

        private void InitTiles()
		{
			_collider = GetComponent<BoxCollider2D>();
			Vector2 size = _collider.size;
			_sizeX = (int)size.x;
			_sizeY = (int)size.y;
            Vector2 boundMin = _collider.offset - size / 2;
			var tileVectorBase = boundMin + Vector2.one / 2;
			_tiles = new Vector2[_sizeX, _sizeY];
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					_tiles[x, y] = new Vector2(x, y) + tileVectorBase;
				}
			}
		}

		public List<Vector2> GetMoveCells(Vector2 moveVector)
		{
			List<Vector2> moveTiles = new List<Vector2>();
			if (_tiles == null) InitTiles();
			foreach (var tile in _tiles)
			{
				moveTiles.Add(tile + (Vector2) transform.position + moveVector);
			}

			return moveTiles;
		}

		public List<Vector2> GetCheckCells(Vector2 moveVector)
		{
			return GetCheckCells(moveVector, transform.position);
		}

		public List<Vector2> GetCheckCells(Vector2 moveVector, Vector2 initPos)
		{
			List<Vector2> checkTiles = new List<Vector2>();
			var moveCells = GetMoveCells(Vector2.zero);
			if (moveVector == Vector2.zero) return moveCells;
			foreach (var moveCell in moveCells)
			{
				var checkTile = moveCell + moveVector;
				if (!moveCells.Contains(checkTile))
				{
					checkTiles.Add(checkTile);
				}
			}

			return checkTiles;
		}
    }
}