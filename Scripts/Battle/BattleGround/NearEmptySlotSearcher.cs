using System.Collections.Generic;
using UnityEngine;

namespace LordAmbermaze.Battle
{
    public class NearEmptySlotSearcher
    {
        private BattleGround _battleGround;
        private HashSet<Vector2> _ignoredPositions = new HashSet<Vector2>();
        private Queue<Vector2> _frontier = new Queue<Vector2>();
        public NearEmptySlotSearcher(BattleGround battleGround)
        {
            _battleGround = battleGround;
        }
        private static readonly List<Vector2> NeighboursTemplate = new List<Vector2>() {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        private int maxSteps = 1000;
        private bool IsEmptySlot(Vector2 destination) => _battleGround.IsEmptySlot(destination);

        public Vector2 Search(Vector2 destination)
        {
            if (IsEmptySlot(destination))
            {
                return destination;
            }

            AddNeighbours(destination);
            var step = 0;

            while (_frontier.Count > 0 && step++ <= maxSteps)
            {
                var pos = _frontier.Dequeue();
                _ignoredPositions.Add(pos);
                if (IsEmptySlot(pos))
                {
                    return pos;
                }

                AddNeighbours(pos);
            }
            return Vector2.zero;
        }

        private void AddNeighbours(Vector2 destination)
        {
            foreach (var vector in NeighboursTemplate)
            {
                var pos = destination + vector;
                if (!_ignoredPositions.Contains(pos))
                {
                    _frontier.Enqueue(pos);
                }
            }
        }

        //public Vector2 Search(Vector2 destination)
        //{
        //    if (IsEmptySlot(destination))
        //    {
        //        return destination;
        //    }

        //    foreach (var vector in NeighboursTemplate)
        //    {
        //        var pos = destination + vector;
        //        if (IsEmptySlot(pos))
        //        {
        //            return pos;
        //        }
        //    }

        //    foreach (var vector in NeighboursTemplate)
        //    {
        //        var pos = destination + vector;
        //        var nearPos = GetNearEmptyData(pos);
        //        if (nearPos != Vector2.zero)
        //        {
        //            return nearPos;
        //        }
        //    }

        //    return Vector2.zero;
        //}
    }
}