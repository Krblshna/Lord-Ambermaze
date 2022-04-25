using AZ.Core;

namespace LordAmbermaze.Core
{
    public interface IHaveDamageEvent
    {
        Func OnHit { get; set; }
    }
}