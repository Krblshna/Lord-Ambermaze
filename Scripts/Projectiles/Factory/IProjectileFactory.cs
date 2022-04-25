using LordAmbermaze.Battle;
using UnityEngine;

namespace LordAmbermaze.Projectiles
{
	public interface IProjectileFactory
	{
		void Init(IBattleManager battleManager, Transform moveTransform);
        GameObject CreateProjectile(Vector2 attackDirection);
	}
}