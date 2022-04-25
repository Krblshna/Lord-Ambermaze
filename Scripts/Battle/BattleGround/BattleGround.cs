using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.DropRewards;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.Battle
{
	public class BattleGround : MonoBehaviour, IBattleGround
	{
		private readonly Dictionary<Vector2Int, SlotType> _nextMovePositions = new Dictionary<Vector2Int, SlotType>();
		private readonly Dictionary<Vector2Int, TileData> _tilesData = new Dictionary<Vector2Int, TileData>();
		private ICharacterCollider _playerCollider;
		private ITileDetector _tileDetector;
		public Vector2 PlayerNextPos { get; private set; }
        public Transform BattleTransform => gameObject == null ? null : transform;

        public Dictionary<Vector2Int, SlotType> NextMovePositions => _nextMovePositions;
        public Dictionary<Vector2Int, TileData> TilesData => _tilesData;
        private NearEmptySlotSearcher _nearEmptySlotSearcher;

        private Func onDisable;
        public void SubscribeOnDisable(Func func)
        {
            onDisable += func;
        }

        public void OnDisable()
        {
            onDisable?.Invoke();
		}

		public void Init(ICharacterCollider playerCollider)
		{
			_playerCollider = playerCollider;
			_tileDetector = GetComponent<ITileDetector>();
            _nearEmptySlotSearcher = new NearEmptySlotSearcher(this);
		}

        public T CheckComponent<T>(Vector2 coordinate) where T : class
        {
            return _tileDetector.CheckComponent<T>(coordinate);
        }

		public void OnPlayerMove(Vector2 destination)
		{
			_nextMovePositions.Clear();
            PlayerNextPos = destination;
			MoveTo(destination, SlotType.Player);
		}

        public void Register(IMonster monster)
        {
            var tilesData = monster.GetUpdatedTilesData();
            if (tilesData == null) return;
            foreach (var tileData in tilesData)
            {
                AddTileData(tileData);
            }
		}

		public void SetTileData(IEnumerable<IMonster> monsters)
		{
			foreach (var monster in monsters)
			{
				var tilesData = monster.GetUpdatedTilesData();
				if (tilesData == null) continue;
				foreach (var tileData in tilesData)
				{
					AddTileData(tileData);
				}
			}
		}

		private void AddTileData(TileData tileData)
		{
			var cordStr = tileData.pos.ToVector2Int();
			if (_tilesData.ContainsKey(cordStr))
			{
				var prevTileData = _tilesData[cordStr];
				if (prevTileData.moveConnector != tileData.moveConnector)
				{
					_tilesData[cordStr] = tileData;
					return;
				}
			}
			_tilesData.Add(cordStr, tileData);
		}

		private void RemoveTileData(TileData tileData)
		{
			var cordStr = tileData.pos.ToVector2Int();
			if (!_tilesData.ContainsKey(cordStr)) return;
			var prevTileData = _tilesData[cordStr];
			if (prevTileData.moveConnector == tileData.moveConnector)
			{
				_tilesData.Remove(cordStr);
			}
		}

		public TileData GetTileData(Vector2 destination)
		{
			var coordinateStr = destination.ToVector2Int();
			if (_tilesData.TryGetValue(coordinateStr, out var tileData))
			{
				return tileData;
			}
			return _tileDetector.GetTileData(destination);
		}

        public bool IsEmptySlot(Vector2 destination)
        {
            var tileData = GetTileData(destination);
            var groundTileData = GetGroundTileData(destination);
			return tileData.slotType == SlotType.Empty && groundTileData.slotType == SlotType.Empty;
        }

        public Vector2 GetNearEmptyData(Vector2 destination)
        {
            return _nearEmptySlotSearcher.Search(destination);
        }

        public TileData GetGroundTileData(Vector2 destination)
        {
            return _tileDetector.GetTileData(destination);
        }

		public ICharacterCollider GetTileCharCollider(Vector2 pos)
		{
			var slotType = CheckNextMovePos(pos);
			if (slotType == SlotType.Player) return _playerCollider;
			var tileData = GetTileData(pos);
			return tileData.moveConnector?.CharacterCollider;
		}

		public void UpdateTileData(TileData oldTileData, TileData newTileData)
		{
			RemoveTileData(oldTileData);
			AddTileData(newTileData);
		}

		public void UpdateTileData(List<TileData> oldTilesData, List<TileData> newTilesData)
		{
			foreach (var tileData in oldTilesData)
			{
				RemoveTileData(tileData);
			}

			foreach (var tileData in newTilesData)
			{
				AddTileData(tileData);
			}
		}

		public SlotType CheckNextMovePos(Vector2 destination)
		{
			var coordStr = destination.ToVector2Int();
			if (_nextMovePositions.TryGetValue(coordStr, out SlotType slotType))
			{
				return slotType;
			}
			return SlotType.Empty;
		}

		public void MoveTo(Vector2 destination, SlotType slotType)
		{
			var coordStr = destination.ToVector2Int();
			//FIXME
			if (_nextMovePositions.ContainsKey(coordStr)) return;
			_nextMovePositions.Add(coordStr, slotType);
		}


		public void MoveTo(List<Vector2> destinations, SlotType slotType)
		{
			foreach (var destination in destinations)
			{
				var coordStr = destination.ToVector2Int();
				//FIXME
				if (_nextMovePositions.ContainsKey(coordStr)) return;
				_nextMovePositions.Add(coordStr, slotType);
			}
		}

		public void RequireMovement(List<Vector2> curPoses, SlotType slotType)
		{
			foreach (var curPos in curPoses)
			{
				var coordStr = curPos.ToVector2Int();
				if (!_nextMovePositions.ContainsKey(coordStr)) continue;
				var cellSlotType = _nextMovePositions[coordStr];
				if (slotType != cellSlotType) continue;
				_nextMovePositions.Remove(coordStr);
			}
		}

		public void Unregister(List<TileData> oldTilesData)
		{
			foreach (var tileData in oldTilesData)
			{
				RemoveTileData(tileData);
			}
		}
	}
}