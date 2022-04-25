using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Effects
{
    public class OneTimeEffect : MonoBehaviour, IEffect
    {

        private Animator _anim;
        private static readonly int Show = Animator.StringToHash("show");
        private IEffectPool _effectPool;

        public GameObject GetObj => gameObject;

        public void Init(IEffectPool effectPool)
        {
            _effectPool = effectPool;
        }

        void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
        }
        public void Execute()
        {
            _anim.SetTrigger(Show);
        }

        public void OnEffectFinish()
        {
            _effectPool.Free(this);
        }
    }
}