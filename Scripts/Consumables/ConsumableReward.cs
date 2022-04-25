using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Consumables
{
    public class ConsumableReward : ActiveObject
    {
        [SerializeField] private bool _oneTimeReward;
        [SerializeField] private Reward reward;

        private void Start()
        {
            if (used_id() && _oneTimeReward)
            {
                Destroy(gameObject);
            }
        }
        protected override void Activate(Collider2D collider)
        {
            if (!collider.CompareTag(Tags.Player)) return;
            save_id();
            reward.Claim();
            Destroy(gameObject);
        }
    }
}