using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Animations
{
	public class AnimationManager : MonoBehaviour, IAnimManager, IHaveOnDeathEvent
	{
		[SerializeField]
		private Transform scaleTransform;
        [SerializeField] private bool InitLeft = true;
        [SerializeField] private bool _flipable = true;
        [SerializeField] private bool _stopOnDeath = false;
		private readonly Dictionary<AnimTypes, string> _animTypesDict = new Dictionary<AnimTypes, string>()
		{
			{AnimTypes.attack, AnimName.Attack},
            {AnimTypes.prepare, AnimName.Prepare},
			{AnimTypes.move, AnimName.Move},
			{AnimTypes.idle, AnimName.Idle},
            {AnimTypes.move_left, AnimName.MoveLeft},
            {AnimTypes.move_down, AnimName.MoveDown},
            {AnimTypes.move_right, AnimName.MoveRight},
            {AnimTypes.move_up, AnimName.MoveUp},
            {AnimTypes.attack_left, AnimName.AttackLeft},
            {AnimTypes.attack_down, AnimName.AttackDown},
            {AnimTypes.attack_right, AnimName.AttackRight},
            {AnimTypes.attack_up, AnimName.AttackUp},
            {AnimTypes.kick, AnimName.Kick}
		};

        private readonly Dictionary<AnimParam, string> _animParamsDict = new Dictionary<AnimParam, string>()
        {
            {AnimParam.agro, AnimParamName.Agro}
        };
		protected Animator _animator;
		private Func _animAction;

		protected virtual void Awake()
		{
			_animator = GetComponent<Animator>();
		}

        bool hasParameter(string paramName)
        {
            foreach (AnimatorControllerParameter param in _animator.parameters)
            {
                if (param.name == paramName) return true;
            }
            return false;
        }

		public void SetDirection(Vector2 direction)
        {
            if (hasParameter("direction_x"))
            {
                _animator.SetInteger("direction_x", (int)direction.x);
            }

            if (hasParameter("direction_y"))
            {
                _animator.SetInteger("direction_y", (int)direction.y);
            }
        }

		public void Play(AnimTypes animType, Func animAction=null)
		{
			_animAction = animAction;
			if (_animTypesDict.TryGetValue(animType, out var animName))
			{
				_animator.SetTrigger(animName);
			}
		}

        public void SetParameter(AnimParam animParam, bool active)
        {
            if (_animParamsDict.TryGetValue(animParam, out var animParamName))
            {
                _animator.SetBool(animParamName, active);
            }
        }

		public bool IsRight()
		{
            if (InitLeft)
            {
                return scaleTransform.localScale.x > 0;
			}
            else
            {
                return scaleTransform.localScale.x < 0;
			}
		}

		public void Turn(Vector2 direction)
		{
			if (!Mathf.Approximately(direction.x, 0))
			{
				CheckValue(direction.x);
			}
			//else
			//{
			//	CheckValue(direction.y);
			//}
		}

		private void CheckValue(float axisVal)
		{
			if (axisVal < 0 && IsRight())
			{
				Flip();
				return;
			}

			if (axisVal > 0 && !IsRight())
			{
				Flip();
			}
		}

		public virtual void Flip()
        {
            if (!_flipable) return;
			//Debug.Log($"flip {Time.time}");
			var sc = scaleTransform.localScale;
			scaleTransform.localScale = new Vector3(-sc.x, sc.y, sc.z);
		}

		public void AnimAction()
		{
			_animAction?.Invoke();
			_animAction = null;
		}

        public void OnDeathEvent()
        {
            if (_stopOnDeath)
            {
                _animator.enabled = false;
            }
        }
    }
}