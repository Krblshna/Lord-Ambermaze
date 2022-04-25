using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Player;
using LordAmbermaze.Projectiles;
using LordAmbermaze.TurnableObjects;
using UnityEngine;

namespace LordAmbermaze.Battle
{
	public class TutorBattleManager : BattleManager
    {
        protected override bool _isTutor { get; } = true;

        public override void CheckLevelComplete()
       {

       }
    }
}