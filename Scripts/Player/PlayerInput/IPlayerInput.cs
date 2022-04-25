using UnityEngine;

namespace LordAmbermaze.Player
{
	public interface IPlayerInput
	{
		void CustomUpdate();
		bool Left { get;  }
		bool Right { get; }
		bool Up { get; }
		bool Down { get; }
		bool Skip { get; }
        bool Back { get; }
		bool Skill1 { get; }
        bool Skill2 { get; }
        bool Skill3 { get; }
        bool Skill4 { get; }
		bool UsedSkill { get;}
        bool ActivateSkill { get; }
    }
}