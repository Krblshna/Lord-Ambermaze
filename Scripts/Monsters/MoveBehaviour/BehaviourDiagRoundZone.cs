using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.AI;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Monsters.MoveBehaviour
{
	public class BehaviourDiagRoundZone : MonoBehaviour, IMoveBehaviour
	{
		[SerializeField] private bool _clockwise;
		private Vector2 _moveDirection;
        public int GetMaxMoves() => _moveDirection.IntLength();

        private Transform _moveTransform;
        private IColliderZone _colliderZone;
        private IAnimManager _animManager;
        private List<Vector2Int> _zone;
        private IEnumerable<SlotType> _obstacleSlots;
        private AIManager _aIManager;
        private bool _wasOutInitZone;
        private bool _firstTimeInZone;

        private static readonly List<EMoveDirection> NeighboursTemplate = new List<EMoveDirection>() {
            EMoveDirection.LeftUp,
            EMoveDirection.LeftDown,
            EMoveDirection.RightUp,
            EMoveDirection.RightDown
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
            CalcInitMoveVector();
        }

        public void OnChangeState(bool active)
        {
            if (active)
            {
                CalcInitMoveVector();
            }
        }

        private Vector2 CalcInitMoveVector()
        {
            var tiles = _colliderZone.Tiles;
            var direction = EMoveDirection.None;
            if (AccessibleMove(EMoveDirection.LeftUp))
            {
                if (AccessibleMove(EMoveDirection.RightUp))
                {
                    direction = _clockwise ? EMoveDirection.LeftUp : EMoveDirection.RightUp;
                }
                else
                {
                    direction = _clockwise ? EMoveDirection.LeftDown : EMoveDirection.LeftUp;
                }
            }
            else
            {
                if (AccessibleMove(EMoveDirection.RightUp))
                {
                    direction = _clockwise ? EMoveDirection.RightUp : EMoveDirection.RightDown;
                }
                else
                {
                    direction = _clockwise ? EMoveDirection.RightDown : EMoveDirection.LeftDown;
                }
            }
            _moveDirection = Utils.DirectionToIntVector(direction);
            foreach (var moveVector in NeighboursTemplate)
            {
                if (AccessibleMove(moveVector))
                {
                    return Utils.DirectionToIntVector(moveVector);
                }
            }
            return Vector2.zero;
        }

        private bool AccessibleMove(EMoveDirection direction)
        {
            var moveVector = Utils.DirectionToIntVector(direction);
            var pos = moveVector + transform.position.ToVector2Int();
            return _colliderZone.Tiles.Contains(pos);
        }


        public Vector2 GetMoveVector() => _moveDirection;

		public void Wait()
		{
			//_moveDirection *= -1;
		}

		public void OnMoveDone()
		{
			var rotateDirection = _clockwise ? EMoveDirection.Right : EMoveDirection.Left;
			SetDirection(_moveDirection.RotateInt(rotateDirection));
		}

        public void RecalcMoveDirection()
        {
            if (_firstTimeInZone)
            {
                _firstTimeInZone = false;
                CalcInitMoveVector();
            }
            if (CouldMove())
            {
                if (_wasOutInitZone)
                {
                    _wasOutInitZone = false;
                    _firstTimeInZone = true;
                }
                return;
            }

            _wasOutInitZone = true;
            MoveToNeighZone();
            if (CouldMove())
            {
                if (_wasOutInitZone)
                {
                    _wasOutInitZone = false;
                    _firstTimeInZone = true;
                }
                return;
            }
            MoveToCosestZoneCell();
		}

        private bool CouldMove()
        {
            var tiles = _colliderZone.Tiles;
            var pos = _moveDirection + (Vector2)transform.position;
            return tiles.Contains(pos.ToVector2Int());
        }

        private void MoveToNeighZone()
        {
            _moveDirection = FindMoveDirection();
        }

        private void MoveToCosestZoneCell()
        {
            var curPos = _moveTransform.position.ToVector2Int();
            var closestCell = FindClosestZoneCell();
            _moveDirection = _aIManager.GetDirectionToCell(curPos, closestCell, _obstacleSlots);
        }

        private Vector2Int FindMoveDirection()
        {
            var tiles = _colliderZone.Tiles;
            var rnd = new System.Random();
            foreach (var direction in NeighboursTemplate.OrderBy(item => rnd.Next()))
            {
                var moveVector = Utils.DirectionToIntVector(direction);
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

        public void SetDirection(Vector2 direction)
		{
			_moveDirection = direction;
		}

        
    }
}