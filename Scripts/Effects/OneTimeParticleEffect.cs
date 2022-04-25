using UnityEngine;

namespace LordAmbermaze.Effects
{
    public class OneTimeParticleEffect : MonoBehaviour, IEffect
    {
        private ParticleSystem _particleSystem;
        private IEffectPool _effectPool;

        public GameObject GetObj => gameObject;

        public void Init(IEffectPool effectPool)
        {
            _effectPool = effectPool;
        }

        void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        public void Execute()
        {
            _particleSystem.Play();
        }

        public void OnEffectFinish()
        {
            _effectPool.Free(this);
        }
    }
}