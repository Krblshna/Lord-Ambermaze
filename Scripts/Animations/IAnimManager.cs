using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Animations
{
	public interface IAnimManager
    {
        void SetDirection(Vector2 direction);
		void SetParameter(AnimParam animParam, bool active);
		void Play(AnimTypes animType, Func animAction = null);
		void Turn(Vector2 direction);
		void Flip();
	}
}