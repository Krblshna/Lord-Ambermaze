using AZ.Core;
using AZ.Core.Conditions;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public class HpAmountCondition : HpCondition
    {
        [SerializeField] private int _hpAmount;
        [SerializeField] private comparators _comparator;
        protected override bool ConditionCheck()
        {
            return SimpleCondition.Check(_hpAmount, Health.CurrentHealth, _comparator);
        }
    }
}