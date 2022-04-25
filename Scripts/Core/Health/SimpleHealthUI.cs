using System;
using System.Collections.Generic;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public class SimpleHealthUI : MonoBehaviour, IHealthUI
	{
		[SerializeField]
		private GameObject heartPrefab;

		private int _maxHealth;
		private readonly List<Heart> hearts = new List<Heart>();

		public void UpdateHP(int currentHealth)
		{
			for (var i = 0; i < _maxHealth; i++)
			{
				var heart = hearts[i];
				heart.Set(i + 1 <= currentHealth);
			}
		}

		public void Init(int maxHealth)
		{
			_maxHealth = maxHealth;
			PopulateHearts();
		}

        public void Hide()
        {
            foreach (var heart in hearts)
            {
                heart.Hide();
            }
        }

        private void PopulateHearts()
		{
			hearts.Clear();
			for (var i = 1; i <= _maxHealth; i++)
			{
				var obj = Instantiate(heartPrefab, Vector3.zero, Quaternion.identity, transform);
				obj.transform.localPosition = GetPos2(i);
				var heart = obj.GetComponent<Heart>();
				hearts.Add(heart);
			}
		}

		private Vector2 GetPos(int id)
		{
			float y = -0.3f;
			float minX = -0.6f, maxX = 0.6f;
			var distance = maxX - minX;
			var delta = distance / (_maxHealth + 1);
			return new Vector2(minX + delta * id, y);
		}

        private Vector2 GetPos2(int id)
        {
            float y = -0.467f;
            float minX = 0.445f, delta = 0.3f;
            return new Vector2(minX - delta * (id - 1), y);
        }
	}
}