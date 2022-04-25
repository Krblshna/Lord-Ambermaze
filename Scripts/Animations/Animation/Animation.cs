using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Animations
{
	public abstract class Animation : MonoBehaviour, IAnimation
	{
		public virtual AnimTypes AnimType { get; }
		public virtual string animName { get; }

		private Animator _animator;

		public void Init()
		{
			_animator = GetComponent<Animator>();
		}

		public void Play()
		{
			_animator.Play(animName);
		}
	}
}