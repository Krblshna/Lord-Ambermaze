using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface ITileDetector
	{
		SlotType GetTileSlotType(Vector2 coordinate);
		TileData GetTileData(Vector2 coordinate);
		T CheckComponent<T>(Vector2 position) where T : class;
	}
}