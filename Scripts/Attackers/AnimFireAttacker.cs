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
	public class AnimFireAttacker : MonoBehaviour, IAttacker
	{
		private IAnimManager _animManager;
        private IBlocker _blocker;
        private IProjectileFactory _projectileFactory;

        private Transform _moveTransform;

        private float _time = 0.1f;

        public void Init(IBattleManager battleManager, Transform moveTransform)
        {
            _projectileFactory = moveTransform.GetComponentInChildren<IProjectileFactory>();
            _animManager = moveTransform.GetComponentInChildren<IAnimManager>();
            _projectileFactory.Init(battleManager, moveTransform);
            _blocker = battleManager.Blocker;
            _moveTransform = moveTransform;
        }

        public void Attack(ICharacterCollider charCollider)
        {
        }

        public void Attack(Vector2 attackDirection, Func onAttackMoment = null)
        {
            _blocker.Block(Blocks.Attack);
            _animManager.Play(AnimTypes.attack, () =>
            {
                var projectile = _projectileFactory.CreateProjectile(attackDirection);
                var destination = (Vector2)_moveTransform.position + attackDirection;
                iTween.MoveTo(projectile, iTween.Hash(
                    "position", (Vector3)destination,
                    "time", 0.5f
                ));
                _blocker.Unblock(Blocks.Attack);
            });

        }
	}
}
