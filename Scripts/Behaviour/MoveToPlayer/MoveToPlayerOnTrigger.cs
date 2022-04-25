using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using LordAmbermaze.Core;
using UnityEngine;
using AZ.Core;
using LordAmbermaze.AI;
using LordAmbermaze.Animations;
using LordAmbermaze.Behaviour.BehaviourConditions;
using LordAmbermaze.Effects;

namespace LordAmbermaze.Behaviour
{
    public class MoveToPlayerOnTrigger : MonoBehaviour, IBehaviour
    {
        [SerializeField] protected GameObject _detectEffectObj, _missEffectObj;

        protected IBehaviour _commonBehaviour;
        protected IBehaviour _agroBehaviour;
        protected IBehaviour _currentBehaviour => _iBehaviourCondition.Active ? _agroBehaviour : _commonBehaviour;
        //
        protected IBehaviourCondition _iBehaviourCondition;
        protected IEffect _detectEffect, _missEffect;
        protected IAnimManager _animManager;

        public Vector2 MoveVector => _currentBehaviour.MoveVector;
        public int MovesRemain => _currentBehaviour.MovesRemain;

        public virtual Type GetAgroType()
        {
            return typeof(MoveToPlayerBehaviour);
        }

        public virtual Type GetCommonType()
        {
            return typeof(MoveAttackBehaviour);
        }

        public virtual void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            _commonBehaviour = gameObject.AddComponent(GetCommonType()) as IBehaviour;
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

        protected void OnChangeState()
        {
            if (_iBehaviourCondition.Active)
            {
                _detectEffect?.Execute();
                _agroBehaviour.OnChangeState(true);
                _commonBehaviour.OnChangeState(false);

            }
            else
            {
                _missEffect?.Execute();
                _agroBehaviour.OnChangeState(false);
                _commonBehaviour.OnChangeState(true);
            }
            _animManager.SetParameter(AnimParam.agro, _iBehaviourCondition.Active);

        }

        public void MakeMove()
        {
            _currentBehaviour.MakeMove();
        }
      
        public bool Wait()
        {
            _currentBehaviour.Wait();
            return false;
        }

        public void OnMoveDone()
        {
            _currentBehaviour.OnMoveDone();
        }

        
        public void SpendMove(bool all)
        {
            _agroBehaviour.SpendMove(all);
            _commonBehaviour.SpendMove(all);
        }

        public void RefreshMoveSteps()
        {
            _agroBehaviour.RefreshMoveSteps();
            _commonBehaviour.RefreshMoveSteps();
        }

        public void OnChangeState(bool active)
        {
            
        }
    }
}