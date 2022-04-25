using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Effects
{
    public class EffectsManager : Singleton<EffectsManager>
    {
        [SerializeField] private EffectPool[] _effects;
        private Dictionary<EffectType, EffectPool> _effectsPools;

        public override void Awake()
        {
            base.Awake();
            _effectsPools = _effects.ToDictionary(effect => effect.Type, effect => effect);
            foreach (var effectPool in _effects)
            {
                effectPool.Init(this);
            }
        }

        public void CreateEffect(EffectType effectType, Vector2 pos)
        {
            if (_effectsPools.TryGetValue(effectType, out var effectPool))
            {
                var effect = effectPool.GetEffect();
                var effectObj = effect.GetObj;
                effectObj.transform.position = pos;
                effect.Execute();
            }
        }
    }
}