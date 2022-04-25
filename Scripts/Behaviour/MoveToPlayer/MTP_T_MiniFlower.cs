using System;
using LordAmbermaze.Animations;
using LordAmbermaze.Behaviour.BehaviourConditions;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
    public class MTP_T_MiniFlower : MoveToPlayerOnTrigger, IDependencyBehaviour
    {
        [SerializeField] private GameObject ControlMonster;

        public override void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            var dependentMovement = gameObject.AddComponent<DependentMove>();
            dependentMovement.ControlMonster = ControlMonster;
            _commonBehaviour = dependentMovement;
            _agroBehaviour = gameObject.AddComponent<MoveToPlayerBehaviourInstant>();
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

        public void SetControlMonster(GameObject monsterObj)
        {
            ControlMonster = monsterObj;
        }
    }
}