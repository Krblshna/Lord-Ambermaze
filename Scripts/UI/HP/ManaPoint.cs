using UnityEngine;

namespace LordAmbermaze.UI
{
	public class ManaPoint : MonoBehaviour
	{
		private IHeartChanger _manaChanger;

		private void Awake()
		{
            _manaChanger = GetComponent<IHeartChanger>();
		}

		public void SetActive(bool active)
		{
            _manaChanger.SetActive(active);
		}
	}
}