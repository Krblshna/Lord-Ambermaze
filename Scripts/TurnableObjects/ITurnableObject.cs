using LordAmbermaze.Battle;

namespace LordAmbermaze.TurnableObjects
{
    public interface ITurnableObject
    {
        void Init(IBattleManager battleManager);
        void MakeMove();
    }
}