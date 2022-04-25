using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Effects
{
    public class MultiEffectConditional : MonoBehaviour, IEffect
    {
        [SerializeField] private SlotType[] disabledSlots;
        [SerializeField] private SlotType[] disabledNextSlots;
        [SerializeField]
        private EffectType effectType;
        [SerializeField]
        private EMoveDirection[] _fireDirections;
        private IEnumerable<Vector2> _fireVectors;
        private IBattleGround _battleGround;

        public GameObject GetObj => gameObject;

        public void Init(IEffectPool effectPool)
        {
            throw new System.NotImplementedException();
        }

        private void Awake()
        {
            _battleGround = GetComponentInParent<IBattleGround>();
            _fireVectors = _fireDirections.Select(direction => Utils.DirectionToVector(direction));
        }
        public void Execute()
        {
            foreach (var fireVector in _fireVectors)
            {
                var pos = fireVector + (Vector2)transform.position;
                var tileData = _battleGround.GetTileData(pos);
                var slotType = tileData.slotType;
                var nextSlotType = _battleGround.CheckNextMovePos(pos);
                if (disabledSlots.Contains(slotType) || disabledNextSlots.Contains(nextSlotType)) continue;
                EffectsManager.Instance.CreateEffect(effectType, pos);
            }
        }
    }
}