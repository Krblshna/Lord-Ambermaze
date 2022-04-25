using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Effects
{
    public class MultiEffect : MonoBehaviour, IEffect
    {
        [SerializeField]
        private EffectType effectType;
        [SerializeField]
        private EMoveDirection[] _fireDirections;
        private IEnumerable<Vector2> _fireVectors;

        private void Awake()
        {
            _fireVectors = _fireDirections.Select(direction => Utils.DirectionToVector(direction));
        }

        public GameObject GetObj => gameObject;

        public void Init(IEffectPool effectPool)
        {
            throw new System.NotImplementedException();
        }

        public void Execute()
        {
            foreach (var fireVector in _fireVectors)
            {
                var pos = fireVector + (Vector2)transform.position;
                EffectsManager.Instance.CreateEffect(effectType, pos);
            }
        }
    }
}