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
	public class TutorMainCharacter : MainCharacter
	{
        public override void OnEffect(CellEffect effect)
        {
            if (IsDead) return;
            //_effectHandlingManager.Handle(effect)
            if (effect == CellEffect.drown)
            {
                EffectsManager.Instance.CreateEffect(EffectType.waterSplash, transform.position);
                gameObject.SetActive(false);
                IsDead = true;
            }
            if (effect == CellEffect.toxicDrown)
            {
                EffectsManager.Instance.CreateEffect(EffectType.toxicSplash, transform.position);
                gameObject.SetActive(false);
                IsDead = true;
            }
            if (effect == CellEffect.fall)
            {
                ObjectsEffectManager.Instance.AddEffect(gameObject, ObjectEffectType.fall, () =>
                {
                    IsDead = true;
                    gameObject.SetActive(false);
                });
            }
		}

        protected override void Death()
        {
            IsDead = true;
            _damageHandler.Death(OnDeath);
        }

        protected override void OnDeath()
        {
            gameObject.SetActive(false);
        }
    }
}