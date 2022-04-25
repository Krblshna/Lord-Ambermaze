using LordAmbermaze.Behaviour.BehaviourConditions;

namespace LordAmbermaze.Behaviour
{
    public interface IAgroGroup
    {
        void OnAgro();
        void Unregister(IBehaviourCondition behaviourCondition);
    }
}