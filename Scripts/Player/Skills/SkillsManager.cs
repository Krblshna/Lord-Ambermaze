using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using JetBrains.Annotations;
using LordAmbermaze.Core;
using LordAmbermaze.Interactions;
using UnityEngine;

namespace LordAmbermaze.Player.Skills
{
    public class SkillSlot
    {
        public int _slotNum;
        public SkillType _skillType;

        public SkillSlot(int slotNum, SkillType skillType)
        {
            _slotNum = slotNum;
            _skillType = skillType;
        }
    }
    public class SkillsManager : MonoBehaviour, ISkillsManager
    {
        private readonly SkillSlot[] _skillSlots = new SkillSlot[]{ new SkillSlot(1, SkillType.Bomb)};
        private Dictionary<SkillType, ISkill> _skillsDict;
        public void Init(Transform moveTransform)
        {
            var skills = GetComponentsInChildren<ISkill>();
            foreach (var skill in skills)
            {
                skill.Init(moveTransform);
            }
            _skillsDict = skills.ToDictionary(skill => skill.skillType,
                skill => skill);
        }

        public ISkill TryGetSkill(int slotNum)
        {
            var selectedSlot = _skillSlots.FirstOrDefault(skillSlot => skillSlot._slotNum == slotNum);
            if (selectedSlot == null) return null;
            _skillsDict.TryGetValue(selectedSlot._skillType, out var skill);
            return skill;
        }

        public void ActivateSkill(int slotNum, Func callback)
        {
            var selectedSlot = _skillSlots.FirstOrDefault(skillSlot => skillSlot._slotNum == slotNum);
            if (selectedSlot == null) return;
            if (_skillsDict.TryGetValue(selectedSlot._skillType, out var skill))
            {
                skill.Activate(callback);
            }
        }

        public bool HaveAvailableSkill(int slotNum)
        {
            var skill = TryGetSkill(slotNum);
            if (skill == null) return false;
            if (PlayerState.CurrentMana < 1) return false;
            return skill.IsAvailable();
        }
    }
}
