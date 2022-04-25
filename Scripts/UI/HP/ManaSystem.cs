using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.UI
{
	public class ManaSystem : MonoBehaviour
	{
		[SerializeField] private GameObject _heartPrefab;
		[SerializeField] private Transform _heartsContainer;
		private readonly List<ManaPoint> _hearts = new List<ManaPoint>();

		void Start()
		{
			InitHearts();
			EventManager.StartListening(EventList.ManaChanged, UpdateManaPoints);
		}

		private void InitHearts()
		{
			var maxHp = PlayerState.MaxMana;
			for (var i = 0; i < maxHp; i++)
			{
				CreateHeart();
			}

			UpdateManaPoints();
		}

		public void UpdateManaPoints()
		{
			var maxHp = _hearts.Count;
			var currentHp = PlayerState.CurrentMana;
			for (var i = 0; i < maxHp; i++)
			{
				var heart = _hearts[i];
				heart.SetActive(i + 1 <= currentHp);
			}
		}

		public void CreateHeart()
		{
			var hpObject = Instantiate(_heartPrefab, _heartsContainer);
			var heart = hpObject.GetComponent<ManaPoint>();
			_hearts.Add(heart);
		}
	}
}