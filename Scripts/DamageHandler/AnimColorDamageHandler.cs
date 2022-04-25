using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.DamageHandler
{
	public class AnimColorDamageHandler : MonoBehaviour, IDamageHandler
	{
		[SerializeField] private SpriteRenderer _renderer;
		[SerializeField] private Color _damageColor, _healColor = Color.green;
        private Animator _anim;
		private Color _deathColor = Color.black, _emptyColor;
		private Color _initColor;
		private float _damageTime = 0.1f;
        private float _deathTime = 0.3f;
		private float _emptyTime = 0.25f;

        public void Init(Transform moveTransform)
        {
            _anim = moveTransform.GetComponentInChildren<Animator>();
        }

		private void Awake()
		{
			_initColor = _renderer.color;
			_emptyColor = _deathColor;
			_emptyColor.a = 0;
		}

		public virtual void GetHit()
        {
            if (_anim.ContainsParam(AnimName.Damage))
            {
                AnimGetHit(); 
            }
            else
            {
                ColorGetHit();
			}
        }

        private void ColorGetHit()
        {
            iTween.ColorTo(_renderer.gameObject, _damageColor, _damageTime);
            iTween.ColorTo(_renderer.gameObject, iTween.Hash(
                "color", _initColor,
                "time", _damageTime,
                "delay", _damageTime
            ));
		}

        private void AnimGetHit()
        {
            _anim.SetTrigger(AnimName.Damage);
		}

		public void Heal()
		{
			iTween.ColorTo(_renderer.gameObject, _healColor, _damageTime);
			iTween.ColorTo(_renderer.gameObject, iTween.Hash(
				"color", _initColor,
				"time", _damageTime,
				"delay", _damageTime
			));
		}

        public void ColorDeath(Func callback)
        {
            iTween.ColorTo(_renderer.gameObject, _damageColor, _damageTime);
            iTween.ColorTo(_renderer.gameObject, iTween.Hash(
                "color", _deathColor,
                "time", _deathTime,
                "delay", _damageTime
            ));
            iTween.ColorTo(_renderer.gameObject, iTween.Hash(
                "color", _emptyColor,
                "time", _emptyTime,
                "delay", _deathTime + _damageTime
            ));
            Utils.SetTimeOut(() =>
            {
                callback?.Invoke();
            }, _damageTime + _deathTime + _emptyTime);
		}

        private void AnimDeath(Func callback)
        {
            _anim.SetTrigger(AnimName.Death);
            //iTween.ColorTo(_renderer.gameObject, _damageColor, _damageTime);
            iTween.ColorTo(_renderer.gameObject, iTween.Hash(
                "color", _deathColor,
                "time", _deathTime,
                "delay", _deathTime
            ));
            iTween.ColorTo(_renderer.gameObject, iTween.Hash(
                "color", _emptyColor,
                "time", _emptyTime,
                "delay", _deathTime * 2
            ));
            Utils.SetTimeOut(() =>
            {
                callback?.Invoke();
            }, _deathTime * 2 + _emptyTime);
        }

		public void Death(Func callback)
		{
            if (_anim.ContainsParam(AnimName.Death))
            {
                AnimDeath(callback);
            }
            else
            {
                ColorDeath(callback);
            }
        }
    }
}