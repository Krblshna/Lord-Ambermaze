using System;
using AZ.Core;
using AZ.Core.UUID;
using LordAmbermaze.Core;
using LordAmbermaze.DropRewards;
using UnityEngine;

namespace LordAmbermaze.Interactions
{
    public class SkillMonument : UUID, IInteractable, IPreventGoNext
    {
        [SerializeField] private Reward _reward;
        private Animator _anim;
        private IDropReward _dropReward;
        private static readonly int Active = Animator.StringToHash("active");
        public InteractibleType InteractType => InteractibleType.poke;
        public Vector2 Pos => transform.position;
        public bool Available { get; private set; } = true;
        public bool IsCleared() => used_id();
        private bool _needClaimReward;

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
            _dropReward = GetComponentInChildren<IDropReward>();
            if (used_id())
            {
                Available = false;
                _anim.SetBool(Active, false);
            }

            _anim.enabled = true;
            EventManager.StartListening(EventList.TurnFinished, CheckReward);
        }

        private void CheckReward()
        {
            if (!_needClaimReward) return;
            _needClaimReward = false;
            _reward?.Claim();
            save_id();
            EventManager.TriggerEvent(EventList.TargetCompleted);
        }

        public void Activate()
        {
            Available = false;
            _anim.SetBool(Active, false);
            SoundManager.PlaySound(SoundType.skill_reward);
            _needClaimReward = true;
            
        }

        public void OnOpened()
        {
            _dropReward.CreateReward();
        }

    }
}