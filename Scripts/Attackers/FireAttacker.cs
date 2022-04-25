using System.Collections;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using LordAmbermaze.Projectiles;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class FireAttacker : MonoBehaviour, IAttacker
	{
        private float _time = 0.1f;
        private IProjectileFactory _projectileFactory;

        public void Init(IBattleManager battleManager, Transform moveTransform)
        {
            _projectileFactory = moveTransform.GetComponentInChildren<IProjectileFactory>();
            _projectileFactory.Init(battleManager, moveTransform);
        }

        public void Attack(ICharacterCollider charCollider)
        {
        }

        public void Attack(Vector2 attackDirection, Func onAttackMoment = null)
        {
            _projectileFactory.CreateProjectile(attackDirection);
        }
	}
}
