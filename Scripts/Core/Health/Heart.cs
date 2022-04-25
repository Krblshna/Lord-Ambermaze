using UnityEngine;

namespace LordAmbermaze.Core
{
	public class Heart : MonoBehaviour
	{
		[SerializeField] private Color _activeColor, _inactiveColor;
		private SpriteRenderer renderer;

		private void Awake()
		{
			renderer = GetComponent<SpriteRenderer>();
		}

		public void Set(bool active)
		{
			renderer.color = active ? _activeColor : _inactiveColor;
		}

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}