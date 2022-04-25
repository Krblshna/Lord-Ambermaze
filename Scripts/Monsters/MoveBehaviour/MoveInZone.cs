using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.AI;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Monsters.MoveBehaviour
{
    public class MoveInZone : MonoBehaviour, IMoveBehaviour
    {
        private Transform _moveTransform;
        private IColliderZone _colliderZone;
        private IAnimManager _animManager;
        private Vector2 _moveDirection;
        private List<Vector2Int> _zone;
        private IEnumerable<SlotType> _obstacleSlots;
        private AIManager _aIManager;

        private static readonly List<Vector2Int> NeighboursTemplate = new List<Vector2Int>() {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down
        };
        public void Init(Transform moveTransform, IEnumerable<SlotType> obstacleSlots)
        {
            _colliderZone = moveTransform.GetComponentInChildren<IColliderZone>();
            _colliderZone.Init();
            _zone = _colliderZone.Tiles.ToList();
            _animManager = GetComponentInChildren<IAnimManager>();
            _moveTransform = moveTransform;
            _aIManager = transform.GetComponentInParent<AIManager>();
            _obstacleSlots = obstacleSlots;
        }

        public void OnChangeState(bool active)
        {
            
        }

        public Vector2 GetMoveVector() => _moveDirection;

        public int GetMaxMoves() => 1;

        public void Wait()
        {
        }

        public void OnMoveDone() { }
        public void SetDirection(Vector2 direction)
        {
        }

        public void RecalcMoveDirection()
        {
            _moveDirection = FindMoveDirection();
            if (_moveDirection == Vector2Int.zero)
            {
                var curPos = _moveTransform.position.ToVector2Int();
                var closestCell = FindClosestZoneCell(curPos);
                _moveDirection = _aIManager.GetDirectionToCell(curPos, closestCell, _obstacleSlots);
            }
        }

        private Vector2Int FindMoveDirection()
        {
            var tiles = _colliderZone.Tiles;
            var rnd = new System.Random();
            foreach (var moveVector in NeighboursTemplate.OrderBy(item => rnd.Next()))
            {
                var pos = moveVector + transform.position.ToVector2Int();
                if (tiles.Contains(pos))
                {
                    return moveVector;
                }
            }
            return Vector2Int.zero;
        }

        private Vector2Int FindClosestZoneCell(Vector2Int curPos)
        {
            var tiles = _colliderZone.Tiles;
            float minDistance = float.PositiveInfinity;
            Vector2Int closestCell = Vector2Int.zero;
            foreach (var zoneTile in tiles)
            {
                var distance = Vector2Int.Distance(curPos, zoneTile);
                if (distance < minDistance)
                {
                    closestCell = zoneTile;
                }
            }
            return closestCell;
        }

        
    }
}