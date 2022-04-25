using System;
using AZ.Core.UUID;
using LordAmbermaze.Core;
using LordAmbermaze.DropRewards;
using UnityEngine;

namespace LordAmbermaze.Interactions
{
    public class Chest : UUID, IInteractable
    {
        private Animator _anim;
        private IDropReward _dropReward;
        private static readonly int Open = Animator.StringToHash("open");
        public InteractibleType InteractType => InteractibleType.kickable;
        public Vector2 Pos => transform.position;
        public bool Available { get; private set; } = true;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _dropReward = GetComponentInChildren<IDropReward>();
            _anim.enabled = false;
            if (used_id())
            {
                Activate();
            }

            _anim.enabled = true;
        }

        public void Activate()
        {
            Available = false;
            _anim.SetTrigger(Open);
        }

        public void OnOpened()
        {
            save_id();
            SoundManager.PlaySound(SoundType.chest_open);
            _dropReward.CreateReward();
        }
    }
}