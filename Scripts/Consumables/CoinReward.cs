using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.Consumables
{
    public class CoinReward : ActiveObject
    {
        [SerializeField] private int amount = 1;
        private Reward _reward;

        private void Awake()
        {
            _reward = new Reward(StorageType.Resources, ResType.Gold, amount);
        }

		protected override void Activate(Collider2D collider)
        {
            if (!collider.CompareTag(Tags.Player)) return;
            SoundManager.PlaySound(SoundType.collect_coin);
            _reward.Claim();
            Destroy(gameObject);
        }
	}
}