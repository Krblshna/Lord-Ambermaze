using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Effects
{
	public interface IEffect
	{
		GameObject GetObj { get; }
		void Execute();
        void Init(IEffectPool effectPool);
    }
}