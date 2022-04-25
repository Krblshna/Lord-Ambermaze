using LordAmbermaze.Core;

namespace LordAmbermaze.Animations
{
	public interface IAnimation
	{
		AnimTypes AnimType { get; }
		void Play();
		void Init();
	}
}