using LordAmbermaze.Core;

namespace LordAmbermaze.EffectHandler
{
    public interface IEffectHandler
    {
        CellEffect CellEffectType { get; }
        void Handle();
    }
}