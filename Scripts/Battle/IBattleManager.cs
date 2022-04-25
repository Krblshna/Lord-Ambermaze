using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using LordAmbermaze.Projectiles;
using UnityEngine;

namespace LordAmbermaze.Battle
{
	public interface IBattleManager
	{
		IBlocker Blocker { get; }
		IBattleGround BattleGround { get; }
        

		void RequireMovement(IMonster battleMonster, List<Vector2> curPoses, SlotType slotType);
		void RequireMovement(IProjectile projectile, List<Vector2> curPoses, SlotType slotType);
		void Unregister(IProjectile projectile);
		void Unregister(IMonster monster);
		void PlayerNextPos(Vector2 newPos);
		void Register(IProjectile projectile);
        void Register(IMonster monster);

		void CheckLevelComplete();
        bool IsSave();

    }
}