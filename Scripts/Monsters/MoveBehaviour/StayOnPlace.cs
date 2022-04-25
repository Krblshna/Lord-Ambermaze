using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Monsters.MoveBehaviour
{
	public class StayOnPlace : MonoBehaviour, IMoveBehaviour
	{
		private Vector2 _moveDirection = Vector2.zero;

		public Vector2 GetMoveVector() => _moveDirection;
        public int GetMaxMoves() => _moveDirection.IntLength();

		public void Wait()
		{
		}

		public void OnMoveDone()
		{
		}

        public void RecalcMoveDirection()
        {
            
        }

        public void SetDirection(Vector2 direction)
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