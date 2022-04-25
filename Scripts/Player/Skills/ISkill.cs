using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Player.Skills
{
    public interface ISkill
    {
        SkillType skillType { get; }
        void Init(Transform moveTransform);
        void Activate(Func callback);
        bool IsAvailable();
    }
}