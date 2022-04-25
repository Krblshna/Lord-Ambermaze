using LordAmbermaze.Core;
using System.Collections.Generic;
using System.Linq;

namespace LordAmbermaze.InteractableSlots
{
	public abstract class AInteractableSlots
	{
		public SlotType SlotType;
		public bool AttackObstacles { get; protected set; }
		protected SlotType[] _walkThroughSlotTypes;
		protected SlotType[] _obstacleSlotTypes;
		protected SlotType[] _attackableSlotTypes;
		protected SlotType[] _connectableSlotTypes;

		public bool HaveObstacleSlot(IEnumerable<SlotType> slotTypes)
		{
			return slotTypes.Any(slotType => _obstacleSlotTypes.Contains(slotType));
		}

		public bool HaveConnectableSlot(IEnumerable<SlotType> slotTypes)
		{
			return slotTypes.Any(slotType => _connectableSlotTypes.Contains(slotType));
		}

		public bool HaveAttackableSlot(IEnumerable<SlotType> slotTypes)
		{
			return slotTypes.Any(slotType => _attackableSlotTypes.Contains(slotType));
		}
        public bool HaveAttackableSlot(SlotType slotType)
        {
            return _attackableSlotTypes.Contains(slotType);
        }

		public bool AreSlotsEmpty(IEnumerable<SlotType> slotTypes)
		{
			return slotTypes.All(slotType => _walkThroughSlotTypes.Contains(slotType));
		}

        public IEnumerable<SlotType> GetObstacleSlots()
        {
            return _obstacleSlotTypes;
        }
    }
}