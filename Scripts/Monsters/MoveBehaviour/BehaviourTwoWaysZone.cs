using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using System.Collections.Generic;
using System.Linq;
using LordAmbermaze.AI;
using UnityEngine;

namespace LordAmbermaze.Monsters.MoveBehaviour
{
	public class BehaviourTwoWaysZone : MonoBehaviour, IMoveBehaviour
	{
		[SerializeField]
		private EMoveDirection _direction;
		[SerializeField, Range(1, 5)]
		private int _moveLength = 1;

        private Transform _moveTransform;
        private IColliderZone _colliderZone;
        private IAnimManager _animManager;
        private Vector2 _moveDirection;
        private List<Vector2Int> _zone;
        private IEnumerable<SlotType> _obstacleSlots;
        private AIManager _aIManager;

        public Vector2 GetMoveVector() => _moveDirection * _moveLength;
        public int GetMaxMoves() => _moveDirection.IntLength();

        private static readonly List<Vector2Int> NeighboursTemplate = new List<Vector2Int>() {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down
        };

        private void Awake()
		{
			_moveDirection = Utils.DirectionToVector(_direction);
			_animManager = GetComponentInChildren<IAnimManager>();
		}


		public void Wait()
		{
			//_animManager.Flip();
			//SetDirection(_moveDirection * -1);
		}

		public void OnMoveDone() {}
		public void SetDirection(Vector2 direction)
		{
			_moveDirection = direction;
		}

		public void RecalcMoveDirection()
        {
            if (CouldMove()) return;
            FlipMoveVector();
            if (CouldMove()) return;
            MoveToNeighZone();
            if (CouldMove()) return;
            MoveToCosestZoneCell();
        }

        private void MoveToCosestZoneCell()
        {
            var curPos = _moveTransform.position.ToVector2Int();
            var closestCell = FindClosestZoneCell();
            _moveDirection = _aIManager.GetDirectionToCell(curPos, closestCell, _obstacleSlots);
        }

        private void MoveToNeighZone()
        {
            _moveDirection = FindMoveDirection();
        }

        private bool CouldMove()
        {
            var tiles = _colliderZone.Tiles;
            var pos = _moveDirection + (Vector2)transform.position;
            return tiles.Contains(pos.ToVector2Int());
        }

        private void FlipMoveVector()
        {
            _moveDirection = _moveDirection * -1;
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

        private Vector2Int FindClosestZoneCell()
        {
            var curPos = _moveTransform.position.ToVector2Int();
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
    }
}