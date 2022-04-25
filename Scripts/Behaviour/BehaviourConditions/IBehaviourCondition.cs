using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public interface IBehaviourCondition
    {
        event Func OnChangeState;
        bool Active { get; }
        void Init(Transform moveTransform);
        void SetAgroGroup(IAgroGroup agroGroup);
        void ForceAgro();
    }
}