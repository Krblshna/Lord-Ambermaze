using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Player.Skills
{
    public interface ISkillsManager
    {
        bool HaveAvailableSkill(int skillNum);
        void ActivateSkill(int skillNum, Func callback);
        void Init(Transform moveTransform);
    }
}