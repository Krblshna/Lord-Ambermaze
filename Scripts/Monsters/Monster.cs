using System.Collections.Generic;
using AZ.Core;
using AZ.Core.UUID;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using LordAmbermaze.InteractableSlots;
using LordAmbermaze.ObjectEffects;
using LordAmbermaze.Physics;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
	public class Monster : Monster_UUID, IDamageable, IMonster, ICanLongMove, IDestructible, IHaveDamageEvent, IPushable, IEffectInflunced
	{
        [SerializeField] private bool _needBeKilled = true;
        [SerializeField] private SoundType hitSound, deathSound;
		private ICharacterEngine _characterEngine;
		private ICharacterCollider _collider;
		private IBattleManager _battleManager;
		private IDamageHandler _damageHandler;
		private IHealth _health;
        private bool _killed, _destroyed;
		public bool IsDead { get; private set; }
        public Vector2 CurPos => transform.position;
        public bool ShouldBeKilled => _needBeKilled && !WasKilled();
		protected virtual AInteractableSlots InteractableSlots { get; } = new MonsterInteractableSlots();
        public Func OnHit { get; set; }
        public Func OnDeathEvent { get; set; }

        private void OnDisable()
        {
            _destroyed = true;
        }
        public void GetHit(int damage)
        {
            if (_killed) return;
			OnHit?.Invoke();
            if (_health == null) return;
			int curHealth = _health.ChangeHealth(-damage);
			if (curHealth < 1)
            {
                _killed = true;
                SoundManager.PlaySound(deathSound);
				SaveKill();
                Death();
            }
			else
			{
                SoundManager.PlaySound(hitSound);
                _damageHandler.GetHit();
			}
		}

		public void Heal(int amount)
		{
			_damageHandler.Heal();
		}

        void Unregister()
        {
            _battleManager.Unregister(this);
            _characterEngine.CustomDestroy();
		}

        void Death(bool handler = true)
        {
            IsDead = true;
            _health.Hide();
            var deathEventComponents = transform.GetComponentsInChildren<IHaveOnDeathEvent>();
            foreach (var deathEventComponent in deathEventComponents)
            {
				deathEventComponent.OnDeathEvent();
			}
			_characterEngine.Death();
			Unregister();
            _damageHandler.Death(OnDeath);
		}

		void OnDeath()
        {
            if (_destroyed) return;
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }

        public bool Init(IBattleManager battleManager)
        {
            var monsterDisabler = GetComponentInParent<MainMonsterDisabler>();
            bool mainMonsterKilled = false;
            if (monsterDisabler != null)
            {
                mainMonsterKilled = monsterDisabler.used_id();
            }
            if (WasKilled() || mainMonsterKilled)
            {
                gameObject.SetActive(false);
                return false;
            }
			_collider = GetComponentInChildren<ICharacterCollider>();
			var moveable = GetComponentInChildren<IMoveConnector>();
			_battleManager = battleManager;
			_characterEngine = GetComponentInChildren<ICharacterEngine>();
			_damageHandler = GetComponentInChildren<IDamageHandler>();
			_health = GetComponentInChildren<IHealth>();
            var attackCollider = GetComponentInChildren<IAttackCollider>();
            if (attackCollider != null)
            {
                attackCollider.Init(transform);
            }

            _damageHandler.Init(transform);
			_collider.Init(this, moveable, Group.Enemy);
			_characterEngine.Init(battleManager, transform, InteractableSlots);
			_health?.Init(transform);
            return true;
        }

		public void MakeMove()
		{
			if (_health != null &&_health.CurrentHealth < 1) return;
			_characterEngine.MakeMove();
		}

		public void AllMovementsStarted()
		{
			_characterEngine.AllMovementsStarted();
		}

		public void RefreshMoveSteps()
		{
			_characterEngine.RefreshMoveSteps();
		}

		public List<TileData> GetUpdatedTilesData()
		{
			return _characterEngine.GetUpdatedTilesData();
		}

		public void RequireMovement(List<Vector2> moveCells)
		{
			_battleManager.RequireMovement(this, moveCells, SlotType.Enemy);
		}

        public void MakeDestroy()
        {
            Death();
        }

        public void Push(Vector2Int pushDirection)
        {
			_characterEngine.Push(pushDirection);
		}

        public void OnEffect(CellEffect effect)
        {
            if (IsDead) return;
            //_effectHandlingManager.Handle(effect)

            if (effect == CellEffect.drown)
            {
                _health.Kill();
                EffectsManager.Instance.CreateEffect(EffectType.waterSplash, transform.position);
                gameObject.SetActive(false);
                IsDead = true;
                Death(false);
            }
            if (effect == CellEffect.toxicDrown)
            {
                _health.Kill();
                EffectsManager.Instance.CreateEffect(EffectType.toxicSplash, transform.position);
                gameObject.SetActive(false);
                IsDead = true;
                Death(false);
            }
			if (effect == CellEffect.fall)
            {
                ObjectsEffectManager.Instance.AddEffect(gameObject, ObjectEffectType.fall, () =>
                {
					_health.Kill();
                    IsDead = true;
                    gameObject.SetActive(false);
					Death(false);
				});
            }
        }

    }
}