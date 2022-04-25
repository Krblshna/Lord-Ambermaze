using AZ.Core;
using LordAmbermaze.Core;
using Rewired.ComponentControls.Effects;
using UnityEngine;

namespace LordAmbermaze.Movers
{
	public class LinearMover : MonoBehaviour, IMover
	{
		private Vector3 _destination;
		private float cur_speed = 0;
		private int _moveSpeed;
		private float _acceleration = 20f;
		private bool _active;
		private event Func _onMoveFinish;
		private Transform _moveTransform;

		public void Init(Transform moveTransform)
		{
			_moveTransform = moveTransform;
		}

		public void MoveTo(Vector2 destination, Func onMoveFinish = null, int moveSpeed = 1)
		{
			cur_speed = 0;
			_moveSpeed = moveSpeed;
			_destination = destination;
			_active = true;
			_onMoveFinish = onMoveFinish;
		}

		public void ForceDestroy()
		{
			Stop();
		}

		public void Update()
		{
			if (!_active) return;
			cur_speed = Mathf.Clamp(cur_speed + _acceleration * Time.deltaTime, 0, Constants.MOVE_SPEED * _moveSpeed);
			float step = cur_speed * Time.deltaTime;
			_moveTransform.position = Vector2.MoveTowards(_moveTransform.position, _destination, step);
			if (Vector3.Distance(_moveTransform.position, _destination) < 0.001f)
			{
				Stop();
				_moveTransform.position = _destination;
			}
		}

		public void Stop()
		{
			_active = false;
			_onMoveFinish?.Invoke();
		}
	}
}