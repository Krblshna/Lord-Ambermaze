using LordAmbermaze.Core;

namespace LordAmbermaze.InteractableSlots
{
	public class ProjectileInteractableSlots : AInteractableSlots
	{
		public ProjectileInteractableSlots()
		{
			SlotType = SlotType.Projectile;
			_walkThroughSlotTypes = new[]{ SlotType.Empty, SlotType.Enemy, SlotType.Player, SlotType.Wall, SlotType.Lake, SlotType.Pit };
			_obstacleSlotTypes = new []{ SlotType.Wall };
			_attackableSlotTypes = new[] { SlotType.Player, SlotType.Enemy, SlotType.Wall };
			_connectableSlotTypes = new[] { SlotType.Enemy };
			AttackObstacles = true;
		}
	}
}