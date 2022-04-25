using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class AgroGroup : MonoBehaviour, IAgroGroup
    {
        private List<IBehaviourCondition> _behaviourConditions = new List<IBehaviourCondition>();

        private void Awake()
        {
            _behaviourConditions = GetComponentsInChildren<IBehaviourCondition>().ToList();
            foreach (var behaviourCondition in _behaviourConditions)
            {
                behaviourCondition.SetAgroGroup(this);
            }
        }

        public void OnAgro()
        {
            foreach (var behaviourCondition in _behaviourConditions)
            {
                behaviourCondition.ForceAgro();
            }
        }

        public void Unregister(IBehaviourCondition behaviourCondition)
        {
            _behaviourConditions.Remove(behaviourCondition);
        }
    }
}