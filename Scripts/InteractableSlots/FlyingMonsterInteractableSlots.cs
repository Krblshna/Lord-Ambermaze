using LordAmbermaze.Core;

namespace LordAmbermaze.InteractableSlots
{
	public class FlyingMonsterInteractableSlots : AInteractableSlots
	{
		public FlyingMonsterInteractableSlots()
		{
			SlotType = SlotType.Enemy;
			_walkThroughSlotTypes = new[]{ SlotType.Empty, SlotType.Lake, SlotType.Pit };
			_obstacleSlotTypes = new[] { SlotType.Wall };
			_attackableSlotTypes = new[] { SlotType.Player };
			_connectableSlotTypes = new[] { SlotType.Enemy };
		}
	}
}