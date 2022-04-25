using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public class TileData
	{
		public Vector2 pos;
		public IMoveConnector moveConnector;
		public SlotType slotType;

		public TileData(Vector2 pos, IMoveConnector moveConnector, SlotType slotType)
		{
			this.pos = pos;
			this.moveConnector = moveConnector;
			this.slotType = slotType;
		}
	}
}