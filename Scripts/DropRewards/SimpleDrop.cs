using LordAmbermaze.Core;

namespace LordAmbermaze.DropRewards
{
    [System.Serializable]
    public class SimpleDrop
    {
        public DropType _dropType;
        public int _amount;
        public float _dropChance = 1;
    }
}