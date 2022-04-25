using LordAmbermaze.Core;

namespace LordAmbermaze.InteractableSlots
{
	public class SpikeBallInteractableSlots : AInteractableSlots
	{
		public SpikeBallInteractableSlots()
		{
			SlotType = SlotType.Projectile;
			_walkThroughSlotTypes = new[]{ SlotType.Empty, SlotType.Enemy, SlotType.Player };
			_obstacleSlotTypes = new []{ SlotType.Wall, SlotType.Lake, SlotType.Pit };
			_attackableSlotTypes = new[] { SlotType.Player, SlotType.Enemy };
			_connectableSlotTypes = new[] { SlotType.Enemy, SlotType.Projectile };
			AttackObstacles = false;
		}
	}
}