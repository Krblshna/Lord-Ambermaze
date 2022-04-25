using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.DamageHandler
{
	public class SimpleDamageHandler : MonoBehaviour, IDamageHandler
	{
		[SerializeField] private SpriteRenderer _renderer;
		[SerializeField] private Color _damageColor, _healColor = Color.green, _deathColor = Color.black;
		private Color _emptyColor;
		private Color _initColor;
		private float _damageTime = 0.1f;
		private float _deathTime = 0.1f;
		private float _emptyTime = 0.1f;

		private void Awake()
		{
			_initColor = _renderer.color;
			_emptyColor = _deathColor;
			_emptyColor.a = 0;
		}

		public void GetHit()
		{
			iTween.ColorTo(_renderer.gameObject, _damageColor, _damageTime);
			iTween.ColorTo(_renderer.gameObject, iTween.Hash(
				"color", _initColor,
				"time", _damageTime,
				"delay", _damageTime
				));
			//Utils.SetTimeOut(() =>
			//{
			//	iTween.ColorTo(_renderer.gameObject, _initColor, damage_time);
			//}, damage_time);
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

		public void Death(Func callback)
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
				"delay", _deathTime +_damageTime
			));
			Utils.SetTimeOut(() =>
			{
				callback?.Invoke();
			}, _damageTime + _deathTime + _emptyTime);
		}

        public void Init(Transform transform)
        {
        }
    }
}