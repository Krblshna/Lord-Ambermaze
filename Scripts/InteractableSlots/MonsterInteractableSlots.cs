using LordAmbermaze.Core;

namespace LordAmbermaze.InteractableSlots
{
	public class MonsterInteractableSlots : AInteractableSlots
	{
		public MonsterInteractableSlots()
		{
			SlotType = SlotType.Enemy;
			_walkThroughSlotTypes = new[]{ SlotType.Empty };
			_obstacleSlotTypes = new[] { SlotType.Wall, SlotType.Lake, SlotType.Pit };
			_attackableSlotTypes = new[] { SlotType.Player };
			_connectableSlotTypes = new[] { SlotType.Enemy };
		}
	}
}