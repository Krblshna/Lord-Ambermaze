using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using LordAmbermaze.Player;
using LordAmbermaze.ScenesManagement;
using UnityEngine;

namespace LordAmbermaze.Animations
{
	public class PlayerAnimationManager : AnimationManager, ITeleportAnimation
    {
        [SerializeField] private bool tutor;
        private IEffectInflunced _affectedChar;
        protected override void Awake()
		{
			base.Awake();
            _affectedChar = GetComponentInParent<IEffectInflunced>();
            if (!tutor)
            {
                TeleportConnector.Instance.RegisterAnimation(this);
            }

            if (!PlayerState.IsRight)
            {
                Flip();
            }
		}

        public override void Flip()
        {
            base.Flip();
            PlayerState.IsRight = IsRight();
        }

        public void TeleportDepature(Func callback)
        {
            var pos = GetComponentInParent<IPlayer>().CurrentPos;
            EffectsManager.Instance.CreateEffect(EffectType.teleport2, pos);
            Utils.SetTimeOut(() =>
            {
                _affectedChar.OnEffect(CellEffect.teleport);
                EffectsManager.Instance.CreateEffect(EffectType.teleport, pos);
            }, 0.3f);
            Utils.SetTimeOut(callback, 1);
        }

        public void TeleportArrival(Func callback)
        {
            var pos = GetComponentInParent<IPlayer>().CurrentPos;
            _affectedChar.OnEffect(CellEffect.teleportArrival);
            EffectsManager.Instance.CreateEffect(EffectType.teleport2, pos);
            Utils.SetTimeOut(() =>
            {
                EffectsManager.Instance.CreateEffect(EffectType.teleport, pos);
            }, 0.3f);
            Utils.SetTimeOut(callback, 1);
        }
    }
}