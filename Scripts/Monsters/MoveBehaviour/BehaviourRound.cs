using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Monsters.MoveBehaviour
{
	public class BehaviourRound : MonoBehaviour, IMoveBehaviour
	{
		[SerializeField] private bool _clockwise;
		[SerializeField] private EMoveDirection _direction;
		private Vector2 _moveDirection;
        public int GetMaxMoves() => _moveDirection.IntLength();

		private void Awake()
		{
			_moveDirection = Utils.DirectionToVector(_direction);
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
            
        }

        public void SetDirection(Vector2 direction)
		{
			_moveDirection = direction;
		}

        public void Init(Transform moveTransform, IEnumerable<SlotType> obstacleSlots)
        {
        }

        public void OnChangeState(bool active)
        {
            
        }
    }
}