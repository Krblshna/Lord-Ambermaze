using LordAmbermaze.Core;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public class MaxHpCondition : HpCondition
    {
        protected override bool ConditionCheck()
        {
            return Health.CurrentHealth != Health.MaxHealth;
        }
    }
}