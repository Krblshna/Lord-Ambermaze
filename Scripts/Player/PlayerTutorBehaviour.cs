using System.Numerics;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.InteractableSlots;
using LordAmbermaze.Interactions;
using LordAmbermaze.Movers;
using LordAmbermaze.Player.Skills;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace LordAmbermaze.Player
{
	public class PlayerTutorBehaviour : PlayerBehaviour
	{
		protected override bool IsBlocked()
		{
			return CommonBlocker.IsBlocked(CommonBlocks.Loading) 
                   || _blocker.IsBlocked(Blocks.Movement) 
                   || _blocker.IsBlocked(Blocks.IntermediateMovement)
                   || CommonBlocker.IsBlocked(CommonBlocks.Map)
				   || _blocker.IsBlocked(Blocks.Attack)
                   || _blocker.IsBlocked(Blocks.SkillSelection);
		}
    }
}