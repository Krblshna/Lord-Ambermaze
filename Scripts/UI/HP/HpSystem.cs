using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.UI
{
	public class HpSystem : MonoBehaviour
	{
		[SerializeField] private GameObject _heartPrefab;
		[SerializeField] private Transform _heartsContainer;
		private readonly List<HpHeart> _hearts = new List<HpHeart>();

		void Start()
		{
			InitHearts();
            EventManager.StartListening("UpdateMaxHp", OnUpdateMaxHp);
		}

		private void InitHearts()
		{
			var maxHp = PlayerState.MaxHp;
			for (var i = 0; i < maxHp; i++)
			{
				CreateHeart();
			}

			UpdateHearts();
		}

        private void OnUpdateMaxHp()
        {
            var delta = PlayerState.MaxHp - _hearts.Count;
            if (delta > 0)
            {
                for (var i = 0; i < delta; i++)
                {
                    CreateHeart();
                }
			}
            UpdateHearts();
		}

		public void UpdateHearts()
		{
			var maxHp = _hearts.Count;
			var currentHp = PlayerState.CurrentHp;
			for (var i = 0; i < maxHp; i++)
			{
				var heart = _hearts[i];
				heart.SetActive(i + 1 <= currentHp);
			}
		}

		public void CreateHeart()
		{
			var hpObject = Instantiate(_heartPrefab, _heartsContainer);
			var heart = hpObject.GetComponent<HpHeart>();
			_hearts.Add(heart);
		}
	}
}