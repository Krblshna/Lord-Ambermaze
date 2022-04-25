using UnityEngine;

namespace LordAmbermaze.Effects
{
	public class SimpleDestructionEffect : MonoBehaviour, IEffect
	{
		[SerializeField]
		private ParticleSystem particleSystem;

        public GameObject GetObj => gameObject;

        public void Init(IEffectPool effectPool)
        {
            throw new System.NotImplementedException();
        }
		public void Execute()
		{
			particleSystem.transform.parent = null;
			particleSystem.Play();
		}
	}
}