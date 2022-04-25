using AZ.Core;
using AZ.Core.UUID;
using LordAmbermaze.Core;
using LordAmbermaze.DropRewards;
using LordAmbermaze.Interactions;
using LordAmbermaze.Prices;
using LordAmbermaze.Sounds;
using UnityEngine;

namespace LordAmbermaze.Market
{
    public class MarketItem : UUID, IInteractable
    {
        private Animator _anim;
        [SerializeField] private Price _price;
        [SerializeField] private Reward _reward;
        [SerializeField] private bool _multiply;
        private static readonly int Open = Animator.StringToHash("open");
        public InteractibleType InteractType => InteractibleType.poke;
        public bool Available { get; private set; } = true;
        public Vector2 Pos => transform.position;
        private IItemPrice itemPrice;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            itemPrice = GetComponentInChildren<IItemPrice>();
            itemPrice.Init(_price);
            if (used_id())
            {
                gameObject.SetActive(false);
            }
        }

        public void Activate()
        {
            if (_price.EnoughAmount())
            {
                SoundManager.PlaySound(SoundType.buy_item);
                _price.Spend();
                _reward.Claim();
                if (!_multiply)
                {
                    save_id();
                    Available = false;
                    gameObject.SetActive(false);
                }

                _anim.SetTrigger(Open);
            }
            else
            {
                SoundManager.PlaySound(SoundType.error);
                _anim.SetTrigger("error");
            }
        }
    }
}