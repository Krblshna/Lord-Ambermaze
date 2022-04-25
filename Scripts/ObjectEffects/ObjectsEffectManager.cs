using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.ObjectEffects
{
    public class ObjectsEffectManager : Singleton<ObjectsEffectManager>
    {
        private Dictionary<ObjectEffectType, IObjectEffect> _effectsDict;
        public override void Awake()
        {
            base.Awake();
            var effects = GetComponentsInChildren<IObjectEffect>();
            _effectsDict = effects.ToDictionary(effect => effect.Type, effect => effect);
        }
        public void AddEffect(GameObject gameObj, ObjectEffectType effectType, Func callback=null)
        {
            if (_effectsDict.TryGetValue(effectType, out var effect))
            {
                effect.Execute(gameObj, callback);
            }
        }

        public void AddEffectPos(GameObject gameObj, ObjectEffectType effectType, Vector2 pos, Func callback = null)
        {
            if (_effectsDict.TryGetValue(effectType, out var effect))
            {
                effect.ExecutePos(gameObj, pos, callback);
            }
        }
    }
}