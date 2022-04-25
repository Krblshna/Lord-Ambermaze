using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.ObjectEffects
{
    public interface IObjectEffect
    {
        ObjectEffectType Type { get;}
        void Execute(GameObject gameObj, Func callback);
        void ExecutePos(GameObject gameObj, Vector2 pos, Func callback);
    }
}