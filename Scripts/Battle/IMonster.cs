using System.Collections.Generic;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Battle
{
	public interface IMonster
    {
        Vector2 CurPos { get; }
        bool ShouldBeKilled { get; }
        bool Init(IBattleManager battleManager);
		void MakeMove();
		void AllMovementsStarted();
		void RefreshMoveSteps();
		List<TileData> GetUpdatedTilesData();
	}
}