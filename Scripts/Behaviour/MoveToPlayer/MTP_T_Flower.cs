using System;
using LordAmbermaze.Animations;
using LordAmbermaze.Behaviour.BehaviourConditions;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
    public class MTP_T_Flower : MoveToPlayerOnTrigger
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject previewPrefab;
        public override Type GetAgroType()
        {
            return typeof(AgroFlowerBehaviour);
        }

        public override Type GetCommonType()
        {
            return typeof(FlowerMoveBehaviour);
        }

        public override void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            var flowerBehaviour = gameObject.AddComponent<FlowerMoveBehaviour>();
            flowerBehaviour._minionPrefab = prefab;
            flowerBehaviour._minionPreviewPrefab = previewPrefab;
            _commonBehaviour = flowerBehaviour;
            _agroBehaviour = gameObject.AddComponent(GetAgroType()) as IBehaviour;
            //gameObject.AddComponent(typeof(MoveToPlayerBehaviour));
            _animManager = moveTransform.GetComponentInChildren<AnimationManager>();
            _iBehaviourCondition = GetComponent<IBehaviourCondition>();
            if (_detectEffectObj != null)
            {
                _detectEffect = _detectEffectObj.GetComponent<IEffect>();
            }

            if (_missEffectObj != null)
            {
                _missEffect = _missEffectObj.GetComponent<IEffect>();
            }

            _commonBehaviour.Init(moveTransform, characterEngine, tileChecker);
            _agroBehaviour.Init(moveTransform, characterEngine, tileChecker);
            _iBehaviourCondition.Init(moveTransform);

            _iBehaviourCondition.OnChangeState += OnChangeState;
        }
    }
}