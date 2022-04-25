using AZ.Core;
using AZ.Core.Depot;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.EffectHandler;
using LordAmbermaze.Effects;
using LordAmbermaze.ObjectEffects;
using LordAmbermaze.Refresh;
using LordAmbermaze.ScenesManagement;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public class MainCharacter : MonoBehaviour, IDamageable, IBattlePlayer, IPlayer, IHealable, IEffectInflunced
	{
        [SerializeField] private SoundType hitSound, deathSound;
        protected PlayerBehaviour _behaviour;
		private ICharacterCollider _collider;
		protected IDamageHandler _damageHandler;
        private IEffectHandlingManager _effectHandlingManager;
		private IHealth _health;
        private IBattleManager _battleManager;

        public bool IsDead { get; protected set; }

        public Vector2 CurrentPos => transform.position;

		public ICharacterCollider Init(IBattleManager battleManager)
        {
            _battleManager = battleManager;
            _behaviour = GetComponentInChildren<PlayerBehaviour>();
			_collider = GetComponentInChildren<ICharacterCollider>();
			_damageHandler = GetComponentInChildren<IDamageHandler>();
			var moveable = GetComponentInChildren<IMoveConnector>();
			_health = GetComponentInChildren<IHealth>();

            _damageHandler.Init(transform);
            _behaviour.Init(battleManager, transform, null);
			_collider.Init(this, moveable, Group.Ally);
			_health.Init(transform);
            EventManager.StartListening("Player:heal", () => Heal(1));
            EventManager.StartListening("counters:MaxHp", OnMaxHpChange);
            OnMaxHpChange();
            return _collider;
		}

        private void OnMaxHpChange()
        {
            PlayerState.UpdateMaxHp((int)Storage.Instance.get("MaxHp"));
            EventManager.TriggerEvent("UpdateMaxHp");
        }

        public void GetHit(int damage)
        {
            if (IsDead) return;
			int curHealth = _health.ChangeHealth(-damage);
            SoundManager.PlaySound(hitSound);
            if (curHealth < 1)
            {
                SoundManager.PlaySound(deathSound);
                Death();
            }
			else
			{
                _damageHandler.GetHit();
			}
		}

		public void Heal(int delta)
		{
            if (IsDead) return;
            _damageHandler.Heal();
			_health.ChangeHealth(delta);
		}

        protected virtual void Death()
        {
            IsDead = true;
            _behaviour.Death();
			CommonBlocker.Block(CommonBlocks.Loading);
            _damageHandler.Death(OnDeath);
		}

        protected virtual void OnDeath()
		{
            Storage.Instance.loose_progress();
            gameObject.SetActive(false);
            EventManager.TriggerEvent(EventList.Death);
            EventManager.TriggerEvent(EventList.ScreenHide);
			Utils.SetTimeOut(() =>
            {
                EventManager.TriggerEvent($"resource:{ResType.Gold}");
                PlayerState.OnRestart();
                ScenesManager.RestartRoom(() =>
                {
                    EventManager.TriggerEvent(EventList.ScreenShow);
                    CommonBlocker.Unblock(CommonBlocks.Loading);
				});
            }, 1);
        }

        public virtual void OnEffect(CellEffect effect)
        {
            if (IsDead) return;
            //_effectHandlingManager.Handle(effect)

            if (effect == CellEffect.drown)
            {
                SoundManager.PlaySound(SoundType.fall);
                CommonBlocker.Block(CommonBlocks.Loading);
                int curHealth = _health.ChangeHealth(-1);
                EffectsManager.Instance.CreateEffect(EffectType.waterSplash, transform.position);
                gameObject.SetActive(false);
				Utils.SetTimeOut(() =>
                {
                    if (curHealth < 1)
                    {
                        IsDead = true;
                        OnDeath();
                    }
                    else
                    {
                        gameObject.SetActive(true);
                        var prevPos = _battleManager.BattleGround.GetNearEmptyData(PlayerState.PlayerLastPos);
                        CommonBlocker.Unblock(CommonBlocks.Loading);
                        ObjectsEffectManager.Instance.AddEffectPos(gameObject,
                            ObjectEffectType.restore_after_fall,
                            prevPos);
                    }
                }, 0.5f);
            }
            if (effect == CellEffect.toxicDrown)
            {
                SoundManager.PlaySound(SoundType.fall);
                CommonBlocker.Block(CommonBlocks.Loading);
                int curHealth = _health.ChangeHealth(-1);
                EffectsManager.Instance.CreateEffect(EffectType.toxicSplash, transform.position);
                gameObject.SetActive(false);
                Utils.SetTimeOut(() =>
                {
                    if (curHealth < 1)
                    {
                        IsDead = true;
                        OnDeath();
                    }
                    else
                    {
                        gameObject.SetActive(true);
                        var prevPos = _battleManager.BattleGround.GetNearEmptyData(PlayerState.PlayerLastPos);
                        CommonBlocker.Unblock(CommonBlocks.Loading);
                        ObjectsEffectManager.Instance.AddEffectPos(gameObject,
                            ObjectEffectType.restore_after_fall,
                            prevPos);
                    }
                }, 0.5f);
            }
            if (effect == CellEffect.fall)
            {
                SoundManager.PlaySound(SoundType.fall);
                CommonBlocker.Block(CommonBlocks.Loading);
                ObjectsEffectManager.Instance.AddEffect(gameObject, ObjectEffectType.fall, () =>
                {
                    int curHealth = _health.ChangeHealth(-1);
                    if (curHealth < 1)
                    {
                        IsDead = true;
                        gameObject.SetActive(false);
                        OnDeath();
                    }
                    else
                    {
                        var prevPos = _battleManager.BattleGround.GetNearEmptyData(PlayerState.PlayerLastPos);
                        CommonBlocker.Unblock(CommonBlocks.Loading);
                        ObjectsEffectManager.Instance.AddEffectPos(gameObject, 
                            ObjectEffectType.restore_after_fall,
                            prevPos);
                    }
                });
            }
            if (effect == CellEffect.teleport)
            {
                ObjectsEffectManager.Instance.AddEffect(gameObject, ObjectEffectType.teleport);
            }

            if (effect == CellEffect.teleportArrival)
            {
                ObjectsEffectManager.Instance.AddEffect(gameObject, ObjectEffectType.teleportArrival);
            }
        }

    }
}