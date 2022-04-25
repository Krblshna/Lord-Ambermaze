using System;
using System.Collections.Generic;
using System.Linq;
using LordAmbermaze.Battle;
using LordAmbermaze.InteractableSlots;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public class TileChecker : MonoBehaviour, ITileChecker
	{
		private IBodyTiles _bodyTiles;
		private IBattleManager _battleManager;
		private AInteractableSlots _interactableSlots;
        public AInteractableSlots InteractableSlots => _interactableSlots;

		private readonly List<TileData> _currentCheckTilesData = new List<TileData>();
		private readonly HashSet<SlotType> _currentCheckSlotTypes = new HashSet<SlotType>();
		private readonly HashSet<SlotType> _futureCheckSlotTypes = new HashSet<SlotType>();

		public void Init(IBodyTiles bodyTiles, IBattleManager battleManager, AInteractableSlots interactableSlots)
		{
			_bodyTiles = bodyTiles;
			_battleManager = battleManager;
			_interactableSlots = interactableSlots;
		}

		public void UpdateChecker(Vector2 direction)
		{
			var checkTiles = _bodyTiles.GetCheckCells(direction);
			_currentCheckTilesData.Clear();
			_currentCheckSlotTypes.Clear();
			_futureCheckSlotTypes.Clear();
			var battleGround = _battleManager.BattleGround;
			foreach (var checkTile in checkTiles)
			{
				var tileData = battleGround.GetTileData(checkTile);
				_currentCheckTilesData.Add(tileData);
				_currentCheckSlotTypes.Add(tileData.slotType);

				var nextTileSlot = battleGround.CheckNextMovePos(checkTile);
				_futureCheckSlotTypes.Add(nextTileSlot);
			}
		}

		public bool CouldMove()
		{
			var enemyWillGoAway = _interactableSlots.HaveConnectableSlot(_currentCheckSlotTypes)
			                      && IsAllMoved(_currentCheckTilesData);
			var haveObstacles = _interactableSlots.HaveObstacleSlot(_currentCheckSlotTypes);
			return (AreSlotsEmptyNow() || enemyWillGoAway) && AreSlotsEmptyAfter() && (!haveObstacles || _interactableSlots.AttackObstacles);
		}

		public bool IsAllMoved(IEnumerable<TileData> tilesData)
		{
			return tilesData.All(tileData =>
				tileData.moveConnector == null || (tileData.moveConnector != null && tileData.moveConnector.MoveMade));
		}

		public bool CouldAttack()
		{
			var haveAttackableObstacleSlot = _interactableSlots.AttackObstacles && HaveObstacle();
			return _interactableSlots.HaveAttackableSlot(_futureCheckSlotTypes) || haveAttackableObstacleSlot;
		}

		public bool HaveObstacle()
		{
			return _interactableSlots.HaveObstacleSlot(_currentCheckSlotTypes);
		}

        public IEnumerable<SlotType> GetObstacleSlots()
        {
            return _interactableSlots.GetObstacleSlots();
        }

        public bool AreSlotsEmptyNow()
		{
			return _interactableSlots.AreSlotsEmpty(_currentCheckSlotTypes);
		}

		public bool AreSlotsEmptyAfter()
		{
			return _interactableSlots.AreSlotsEmpty(_futureCheckSlotTypes);
		}

		public bool ShouldConnect()
		{
			var haveEnemyOnTheWay = _interactableSlots.HaveConnectableSlot(_currentCheckSlotTypes);
			var someEnemyNotMoved = haveEnemyOnTheWay && !IsAllMoved(_currentCheckTilesData);
			return someEnemyNotMoved && AreSlotsEmptyAfter();
		}

		public IEnumerable<TileData> FilteredCurrentCheckTilesData(Func<TileData, bool> predicate)
		{
			return _currentCheckTilesData.Where(predicate);
		}
	}
}