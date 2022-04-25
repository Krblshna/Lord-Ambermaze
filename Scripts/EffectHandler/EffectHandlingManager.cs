using System.Collections.Generic;
using System.Linq;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.EffectHandler
{
    public class EffectHandlingManager : MonoBehaviour, IEffectHandlingManager
    {
        private IEffectHandler[] _effectHandlers;

        private Dictionary<CellEffect, IEffectHandler> _effectDict =
            new Dictionary<CellEffect, IEffectHandler>();
        void Awake()
        {
            _effectHandlers = GetComponentsInChildren<IEffectHandler>();
            _effectDict = _effectHandlers.ToDictionary(effect => effect.CellEffectType, effect => effect); 
        }
        public void Handle(CellEffect cellEffect)
        {
            if (_effectDict.TryGetValue(cellEffect, out var effectHandler))
            {
                effectHandler.Handle();
            }
        }
    }
}