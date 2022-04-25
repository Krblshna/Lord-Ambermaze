using System;
using System.Collections;
using System.Collections.Generic;
using LordAmbermaze.Battle;
using LordAmbermaze.InteractableSlots;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface ITileChecker
	{
		void Init(IBodyTiles bodyTiles, IBattleManager battleManager, AInteractableSlots interactableSlots);
		void UpdateChecker(Vector2 direction);
		bool CouldMove();
		bool CouldAttack();
		bool ShouldConnect();
		IEnumerable<TileData> FilteredCurrentCheckTilesData(Func<TileData, bool> func);
		bool HaveObstacle();
        AInteractableSlots InteractableSlots { get; }
		IEnumerable<SlotType> GetObstacleSlots();
    }
}