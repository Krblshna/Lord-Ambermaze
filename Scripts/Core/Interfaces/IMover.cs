using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface IMover
	{
		void Init(Transform moveTransform);
		void MoveTo(Vector2 destination, Func onMoveFinish = null, int speed = 1);
		void Stop();
	}
}