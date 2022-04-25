using System;
using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public class AutoMover : MonoBehaviour, IAutoMover
	{
		private Transform _moveTransform;
		private IMover _mover;
		private int moveTimes = 0;
		private Vector2 _direction;
		private Action _callback;
		private IAnimManager _animManager;
		private Vector2 borderValues = new Vector2(8.5f, 5.5f);
		private Dictionary<Vector2, int> _movesToDirection = new Dictionary<Vector2, int>()
		{
			{Vector2.up, 3},
			{Vector2.down, 2},
			{Vector2.left, 3},
			{Vector2.right, 3},
		};

        private Dictionary<Vector2, int> _movesFromDirection = new Dictionary<Vector2, int>()
        {
            {Vector2.up, 3},
            {Vector2.down, 2},
            {Vector2.left, 2},
            {Vector2.right, 2},
        };

		public void Init(Transform moveTransform, IAnimManager animManager)
		{
			_moveTransform = moveTransform;
			_mover = GetComponent<IMover>();
			_animManager = animManager;
		}

		private int GetMovesAmount(bool movementFromLocation)
		{
			var direction = _direction;
			if (movementFromLocation)
			{
				direction *= -1;
			}

            var moveDict = movementFromLocation ? _movesFromDirection : _movesToDirection;

			if (moveDict.TryGetValue(direction, out var movesAmount))
			{
				return movesAmount;
			}

			return 0;
		}

		public void MoveFromLocationBorder(Action callback)
		{
			_callback = callback;
			_direction = GameMaster.LastTransitionDirection;
			_animManager.Turn(_direction);
			moveTimes = GetMovesAmount(true);
			SetInitPosition();
			Repeat();
		}

        public void InstantMove(Vector2 teleportCellGetPos)
        {
			_moveTransform.localPosition = teleportCellGetPos;
		}

        public void MoveToLocationBorder(Action callback)
		{
			_callback = callback;
            _callback += () =>
            {
                _moveTransform.gameObject.SetActive(false);
            };
			_direction = GetDirection();
			_animManager.Turn(_direction);
			moveTimes = GetMovesAmount(false);
			GameMaster.SetTransitionData(_moveTransform.localPosition, _direction);
			Repeat();
		}

		private void SetInitPosition()
		{
			Vector2 pos;
			if (_direction == Vector2.up)
			{
				pos = new Vector2(GameMaster.LastLocalPosition.x, -borderValues.y);
			} else if (_direction == Vector2.down)
			{
				pos = new Vector2(GameMaster.LastLocalPosition.x, borderValues.y);
			}
			else if (_direction == Vector2.right)
			{
				pos = new Vector2(-borderValues.x, GameMaster.LastLocalPosition.y);
			}
			else
			{
				pos = new Vector2(borderValues.x, GameMaster.LastLocalPosition.y);
			}

			_moveTransform.localPosition = pos;
		}

		

		private Vector2 GetDirection()
		{
			float y = _moveTransform.localPosition.y;
			float x = _moveTransform.localPosition.x;
			Vector2 direction;
			if (Mathf.Abs(y) > Mathf.Abs(x))
			{
				direction = y > 0 ? Vector2.up : Vector2.down;
			}
			else
			{
				direction = x > 0 ? Vector2.right : Vector2.left;
			}
			return direction;
		}

		private void Repeat()
		{
			if (moveTimes <= 0)
			{
                _callback?.Invoke();
				return;
			}

			moveTimes--;
			var newPos = _moveTransform.position + (Vector3)_direction;
			Utils.SetTimeOut(() =>
			{
				_animManager.Play(AnimTypes.move);
				_mover.MoveTo(newPos, Repeat);
			}, 0.1f);
		}
	}
}