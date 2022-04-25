using AZ.Core;
using LordAmbermaze.Battle;
using UnityEngine;

namespace LordAmbermaze.Projectiles
{
	public class SimpleProjectileFactory : MonoBehaviour, IProjectileFactory
	{
		[SerializeField] private GameObject _projectilePrefab;
		private Transform _moveTransform;

		public void Init(IBattleManager battleManager, Transform moveTransform)
		{
			_moveTransform = moveTransform;
		}

		public GameObject CreateProjectile(Vector2 attackDirection)
		{
            var position = _moveTransform.position;
            //var destination = position + (Vector3)direction;
            var gameObj = Instantiate(_projectilePrefab, position, Quaternion.identity, _moveTransform.parent);
            var projectile = gameObj.GetComponent<IProjectile>();
            projectile.SetDirection(attackDirection);
            return gameObj;
        }
	}
}