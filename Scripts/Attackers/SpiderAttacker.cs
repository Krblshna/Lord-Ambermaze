using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class SpiderAttacker : BombAttacker
    {
       public override void Attack(Vector2 attackDirection, Func onAttackMoment = null)
		{
            var opponentColliders = GetAttackedColliders();
            DoDamage(opponentColliders);
            _effect?.Execute();
            onAttackMoment?.Invoke();
        }
    }
}