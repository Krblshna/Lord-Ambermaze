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
    public class BehaviourOnTrigger : MonoBehaviour, IBehaviour
    {
        [SerializeField] private GameObject _idleBehaviourObj, _triggeredBehaviourObj;

        private IBehaviour _idleBehaviour;
        private IBehaviour _triggeredBehaviour;
        private IBehaviour _currentBehaviour => _iBehaviourCondition.Active ? _triggeredBehaviour : _idleBehaviour;
        private IBehaviourCondition _iBehaviourCondition;

        public Vector2 MoveVector => _currentBehaviour.MoveVector;
        public int MovesRemain => _currentBehaviour.MovesRemain;

        public void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            _idleBehaviour = _idleBehaviourObj.GetComponent<IBehaviour>();
            _triggeredBehaviour = _triggeredBehaviourObj.GetComponent<IBehaviour>();
            _iBehaviourCondition = GetComponent<IBehaviourCondition>();

            _idleBehaviour.Init(moveTransform, characterEngine, tileChecker);
            _triggeredBehaviour.Init(moveTransform, characterEngine, tileChecker);
            _iBehaviourCondition.Init(moveTransform);

            _iBehaviourCondition.OnChangeState += OnChangeState;
        }

        private void OnChangeState()
        {
            if (_iBehaviourCondition.Active)
            {
                _triggeredBehaviour.OnChangeState(false);
                _idleBehaviour.OnChangeState(true);

            }
            else
            {
                _triggeredBehaviour.OnChangeState(true);
                _idleBehaviour.OnChangeState(false);
            }
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
            _triggeredBehaviour.SpendMove(all);
            _idleBehaviour.SpendMove(all);
        }

        public void RefreshMoveSteps()
        {
            _triggeredBehaviour.RefreshMoveSteps();
            _idleBehaviour.RefreshMoveSteps();
        }

        public void OnChangeState(bool active)
        {
            
        }
    }
}