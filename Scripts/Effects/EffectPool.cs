using System.Collections.Generic;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Effects
{
    [System.Serializable]
    public class EffectPool : IEffectPool
    {
        public EffectType Type;
        public GameObject Obj;
        Stack<IEffect> _availableEffects = new Stack<IEffect>();
        private EffectsManager _effectsManager;

        public IEffect GetEffect()
        {
            if (_availableEffects.Count > 0)
            {
                var effect = _availableEffects.Pop();
                effect.GetObj.SetActive(true);
                return effect;
            }

            return CreateNewEffect();
        }

        private IEffect CreateNewEffect()
        {
            var obj = Object.Instantiate(Obj, Vector3.zero, Quaternion.identity, _effectsManager.transform);
            var effect = obj.GetComponent<IEffect>();
            effect.Init(this);
            return effect;
        }

        public void Free(IEffect effect)
        {
            effect.GetObj.SetActive(false);
            _availableEffects.Push(effect);
        }

        public void Init(EffectsManager effectsManager)
        {
            _effectsManager = effectsManager;
        }
    }
}