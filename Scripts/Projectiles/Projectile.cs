using System.Collections.Generic;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.InteractableSlots;
using LordAmbermaze.Physics;
using UnityEngine;

namespace LordAmbermaze.Projectiles
{
	public class Projectile : MonoBehaviour, IProjectile, ICanLongMove, IDestructible
	{
		private ICharacterEngine _characterEngine;
		private IBattleManager _battleManager;
		private IProjectileGraphic _projectileGraphic;
		private IBattleManager BattleManager => _battleManager ?? (_battleManager = GetComponentInParent<IBattleManager>());

		public void Init()
		{
			_characterEngine = GetComponentInChildren<ICharacterEngine>();
			_characterEngine.Init(BattleManager, transform, new ProjectileInteractableSlots());

			var attackCollider = GetComponentInChildren<IAttackCollider>();
			attackCollider.Init(transform);
		}

		void Start()
		{
			BattleManager.Register(this);
			Init();
		}

		public void MakeMove()
		{
			_characterEngine.MakeMove();
		}

		public void RefreshMoveSteps()
		{
			_characterEngine.RefreshMoveSteps();
		}

		public void AllMovementsStarted()
		{
			_characterEngine.AllMovementsStarted();
		}

		public void SetDirection(Vector2 direction)
		{
			_projectileGraphic = GetComponentInChildren<IProjectileGraphic>();
			_projectileGraphic.SetDirection(direction);

			var moveBehaviour = GetComponentInChildren<IMoveBehaviour>();
			moveBehaviour.SetDirection(direction);
		}

		public void MakeDestroy()
		{
			gameObject.SetActive(false);
			_battleManager.Unregister(this);
		}

		public void RequireMovement(List<Vector2> moveCells)
		{
			_battleManager.RequireMovement(this, moveCells, SlotType.Projectile);
		}
	}
}