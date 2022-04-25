using AZ.Core;
using AZ.Core.UUID;
using LordAmbermaze.Core;
using LordAmbermaze.DropRewards;
using LordAmbermaze.Environment;
using LordAmbermaze.Prices;
using UnityEngine;

namespace LordAmbermaze.Interactions
{
    public class DoorLock : UUID, IInteractable
    {
        private IGate _gate;
        [SerializeField] private GameObject gateObj;
        [SerializeField] private PriceInventory priceInventory;
        private Price price => priceInventory.price;
        private Animator _anim;
        private static readonly int Open = Animator.StringToHash("open");
        public InteractibleType InteractType => InteractibleType.poke;
        public Vector2 Pos => transform.position;
        public bool Available { get; private set; } = true;
        private bool _opened;
        private bool _initialized;

        public bool Opened
        {
            get
            {
                Init();
                return _opened;
            }
            set { _opened = value; }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (_initialized) return;
            _initialized = true;
            _anim = GetComponent<Animator>();
            _gate = gateObj.GetComponent<IGate>();
            _gate.SetLock(this);
            if (used_id())
            {
                Available = false;
                Opened = true;
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            if (used_id())
            {
                gameObject.SetActive(false);
            }
        }

        public void OnUnlockAnimEnd()
        {
            this.gameObject.SetActive(false);
            _gate.OnLockOpen();
            SoundManager.PlaySound(SoundType.door_open);
        }

        public void Activate()
        {
            if (price.EnoughAmount())
            {
                SoundManager.PlaySound(SoundType.use_key);
                save_id();
                price.Spend();
                Available = false;
                Opened = true;
                _anim.SetTrigger(Open);
            }
            else
            {
                SoundManager.PlaySound(SoundType.error);
                _anim.SetTrigger("error");
            }
        }

        public void OnOpened()
        {
            //_gate.OnLockOpen();
            Debug.Log("Open");
        }
    }
}