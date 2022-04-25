using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.ScenesManagement
{
	public class SceneMover : MonoBehaviour
    {
		private const float kXDelta = 19.214f, kYDelta = 12.28f;

		private void Awake()
		{
			var pos = GameMaster.ScenePos;
			transform.position = new Vector3(pos.x * kXDelta, pos.y * kYDelta);
		}
	}
}