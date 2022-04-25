using System.Collections.Generic;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Battle
{
    public class CellEffector : MonoBehaviour, ICellEffector
    {
        private IEffectInflunced _affectedObject;
        private readonly HashSet<string> _tagsSet = new HashSet<string>();
        private readonly List<string> _effectiveTags = new List<string>() { Tags.Lake, Tags.Pit, Tags.Toxic };

        private void Awake()
        {
            _affectedObject = GetComponentInParent<IEffectInflunced>();
        }

        private readonly Dictionary<string, CellEffect> _cellsEffects = new Dictionary<string, CellEffect>()
        {
            { Tags.Lake, CellEffect.drown },
            { Tags.Pit, CellEffect.fall },
            { Tags.Toxic, CellEffect.toxicDrown }
        };


        void OnTriggerEnter2D(Collider2D collider)
        {
            if (_effectiveTags.Contains(collider.tag))
            {
                _tagsSet.Add(collider.tag);
            }
        }

        public void Check()
        {
            foreach (var keyValue in _cellsEffects)
            {
                var tag = keyValue.Key;
                var effect = keyValue.Value;
                if (_tagsSet.Contains(tag))
                {
                    _affectedObject.OnEffect(effect);
                }
            }
            _tagsSet.Clear();
        }
    }
}