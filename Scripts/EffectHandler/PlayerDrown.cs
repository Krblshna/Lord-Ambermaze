using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using LordAmbermaze.ObjectEffects;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.EffectHandler
{
    public class PlayerDrown : MonoBehaviour, IEffectHandler
    {
        public void Handle()
        {
            //CommonBlocker.Block(CommonBlocks.Loading);
            //int curHealth = _health.ChangeHealth(-1);
            //EffectsManager.Instance.CreateEffect(EffectType.waterSplash, transform.position);
            //gameObject.SetActive(false);
            //Utils.SetTimeOut(() =>
            //{
            //    if (curHealth < 1)
            //    {
            //        IsDead = true;
            //        OnDeath();
            //    }
            //    else
            //    {
            //        gameObject.SetActive(true);
            //        Vector2 prevPos = PlayerState.PlayerLastPos;
            //        CommonBlocker.Unblock(CommonBlocks.Loading);
            //        ObjectsEffectManager.Instance.AddEffectPos(gameObject,
            //            ObjectEffectType.restore_after_fall,
            //            prevPos);
            //    }
            //}, 0.5f);
        }

        public CellEffect CellEffectType => CellEffect.drown;
    }
}