using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Battle
{
	public interface IBattleGround
	{
		Transform BattleTransform { get; }
		void SetTileData(IEnumerable<IMonster> monsters);
		SlotType CheckNextMovePos(Vector2 newPos);
		void UpdateTileData(TileData oldTileData, TileData newTileData);
		void UpdateTileData(List<TileData> oldTileData, List<TileData> newTileData);
		void MoveTo(Vector2 newPos, SlotType slotType);
		void MoveTo(List<Vector2> newPoses, SlotType slotType);
		void OnPlayerMove(Vector2 newPos);
		TileData GetTileData(Vector2 pos);
        TileData GetGroundTileData(Vector2 pos);
		ICharacterCollider GetTileCharCollider(Vector2 pos);
		void Init(ICharacterCollider playerCollider);
		void RequireMovement(List<Vector2> curPoses, SlotType slotType);
		void Unregister(List<TileData> oldTilesData);
        Vector2 PlayerNextPos { get; }
        Dictionary<Vector2Int, SlotType> NextMovePositions { get; }
        Dictionary<Vector2Int, TileData> TilesData { get; }

        Vector2 GetNearEmptyData(Vector2 pos);
        T CheckComponent<T>(Vector2 coordinate) where T : class;
        void SubscribeOnDisable(Func func);

        void Register(IMonster monster);

    }
}