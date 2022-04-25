using LordAmbermaze.Battle;
using UnityEngine;

namespace LordAmbermaze.Projectiles
{
	public interface IProjectile
	{
		//void Init(IBattleManager battleManager);
		void MakeMove();
		void RefreshMoveSteps();
		void AllMovementsStarted();
		void SetDirection(Vector2 direction);
	}
}