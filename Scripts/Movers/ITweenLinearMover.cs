using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace LordAmbermaze.Movers
{
	public class ITweenLinearMover : MonoBehaviour, IMover
	{
		private GameObject _moveGameObj;
		private float _time = 0.3f;
		private iTween.EaseType easeType = iTween.EaseType.linear;
		private Func _onMoveFinish;

		public void Init(Transform moveTransform)
		{
			_moveGameObj = moveTransform.gameObject;
		}

		private void onComplete()
		{
			_onMoveFinish?.Invoke();
		}

		public void MoveTo(Vector2 destination, Func onMoveFinish = null, int speed = 1)
		{
			_onMoveFinish = onMoveFinish;
			iTween.MoveTo(_moveGameObj, iTween.Hash(
				"position", (Vector3)destination,
				"time", _time / speed,
				"easetype", easeType,
				"oncomplete", "onComplete",
				"oncompletetarget", gameObject
			));
		}

		public void Stop()
		{
            iTween.Stop(_moveGameObj);
			onComplete();
		}
	}
}