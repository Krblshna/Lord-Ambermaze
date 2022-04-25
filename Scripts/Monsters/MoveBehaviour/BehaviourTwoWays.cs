using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using System.Collections.Generic;
using UnityEngine;

namespace LordAmbermaze.Monsters.MoveBehaviour
{
	public class BehaviourTwoWays : MonoBehaviour, IMoveBehaviour
	{
		[SerializeField]
		private EMoveDirection _direction;
		[SerializeField, Range(1, 5)]
		private int _moveLength = 1;
		private Vector2 _moveDirection;
		private IAnimManager _animManager;

        public Vector2 GetMoveVector() => _moveDirection * _moveLength;
        public int GetMaxMoves() => _moveLength;

		private void Awake()
		{
			_moveDirection = Utils.DirectionToVector(_direction);
			_animManager = GetComponentInChildren<IAnimManager>();
		}


		public virtual void Wait()
		{
            SetDirection(_moveDirection * -1);
            _animManager.SetDirection(_moveDirection);
		}

		public void OnMoveDone() {}
		public void SetDirection(Vector2 direction)
		{
			_moveDirection = direction;
		}

        public void RecalcMoveDirection()
        {
        }

        public void Init(Transform moveTransform, IEnumerable<SlotType> obstacleSlots)
        {
        }

        public void OnChangeState(bool active)
        {
            
        }
    }
}