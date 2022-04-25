using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using UnityEngine;
using LordAmbermaze.AI;
using System.Collections.Generic;

namespace LordAmbermaze.Monsters.MoveBehaviour
{
	public class MoveToPlayer : MonoBehaviour, IAIMoveBehaviour
	{
		private Vector2 _moveDirection;
		private AIManager _aIManager;
		private Transform _moveTransform;
		private IEnumerable<SlotType> _obstacleSlots;

		private void Awake()
		{
			_aIManager = GetComponentInParent<AIManager>();
		}

		public void Init(Transform moveTransform, IEnumerable<SlotType> obstacleSlots)
        {
			_moveTransform = moveTransform;
			_obstacleSlots = obstacleSlots;
		}

		public Vector2 GetMoveVector() => _moveDirection;

		public void Wait()
		{
		}

		public void OnMoveDone() {}
		public void SetDirection(Vector2 direction)
		{
			_moveDirection = direction;
		}

        public void RecalcMoveDirection()
        {
			_moveDirection = _aIManager.GetMoveDirectionToPlayer(_moveTransform, _obstacleSlots);
		}
    }
}