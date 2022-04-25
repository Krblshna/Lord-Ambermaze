using UnityEngine;

namespace LordAmbermaze.UI
{
	public class HpHeart : MonoBehaviour
	{
		private IHeartChanger _heartChanger;

		private void Awake()
		{
			_heartChanger = GetComponent<IHeartChanger>();
		}

		public void SetActive(bool active)
		{
			_heartChanger.SetActive(active);
		}
	}
}