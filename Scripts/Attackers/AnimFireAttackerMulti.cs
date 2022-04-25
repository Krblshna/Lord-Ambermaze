using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using LordAmbermaze.Projectiles;
using UnityEngine;

namespace LordAmbermaze.Attackers
{
	public class AnimFireAttackerMulti : MonoBehaviour, IAttacker
	{
		private IAnimManager _animManager;
        private IBlocker _blocker;
        private IProjectileFactory _projectileFactory;
        [SerializeField]
        private EMoveDirection[] _fireDirections;
        private IEnumerable<Vector2> _fireVectors;
        private Transform _moveTransform;
        private float _time = 0.1f;

        private void Awake()
        {
            _fireVectors = _fireDirections.Select(direction => Utils.DirectionToVector(direction));
        }

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
                foreach (var fireVector in _fireVectors)
                {
                    AttackDirection(fireVector);
                }
                _blocker.Unblock(Blocks.Attack);
            });

        }

        private void AttackDirection(Vector2 fireVector)
        {
            var projectile = _projectileFactory.CreateProjectile(fireVector);
            var destination = (Vector2)_moveTransform.position + fireVector;
            iTween.MoveTo(projectile, iTween.Hash(
                "position", (Vector3)destination,
                "time", 0.5f
            ));
        }
	}
}
