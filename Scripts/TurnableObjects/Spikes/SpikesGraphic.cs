using UnityEngine;

namespace LordAmbermaze.TurnableObjects.Spikes
{
	public class SpikesGraphic : MonoBehaviour, ISpikesGraphics
	{
		private SpriteRenderer renderer;
		[SerializeField]
		private Sprite _wait, _prepare, _attack;

		public void Init()
		{
			renderer = GetComponent<SpriteRenderer>();
		}

		private void SetSprite(Sprite sprite)
		{
			renderer.sprite = sprite;
		}

		public void Prepare()
		{
			SetSprite(_prepare);
		}

		public void Attack()
		{
			SetSprite(_attack);
		}

		public void Wait()
		{
			SetSprite(_wait);
		}
	}
}