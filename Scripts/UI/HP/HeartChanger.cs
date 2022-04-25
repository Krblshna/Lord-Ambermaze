using UnityEngine;
using UnityEngine.UI;

namespace LordAmbermaze.UI
{
	public class HeartChanger : MonoBehaviour, IHeartChanger
	{
		[SerializeField] private Sprite activeSprite, inactiveSprite;
		private Image _renderer;

		private void Awake()
		{
			_renderer = GetComponent<Image>();
		}

		public void SetActive(bool active)
		{
			_renderer.sprite = active ? activeSprite : inactiveSprite;
		}
	}
}