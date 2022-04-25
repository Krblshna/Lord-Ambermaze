using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
    public class ColliderZone : MonoBehaviour, IColliderZone
    {
		public HashSet<Vector2Int> Tiles => _tiles;
        private readonly HashSet<Vector2Int> _tiles = new HashSet<Vector2Int>();

        //Don't use Awake. Tile need change position first in Awake on Scene load
        public void Init()
        {
            InitTiles();
        }

        private void InitTiles()
        {
            var colliders = GetComponentsInChildren<BoxCollider2D>();
            foreach (var coll in colliders)
            {
                Vector2 size = coll.size;
                Vector2 boundMin = coll.offset - size / 2;
                var tileVectorBase = boundMin + Vector2.one / 2 + (Vector2)coll.transform.position;
                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        var pos = new Vector2(x, y) + tileVectorBase;
                        _tiles.Add(pos.ToVector2Int());
                    }
                }

                coll.gameObject.SetActive(false);
            }
        }
	}
}