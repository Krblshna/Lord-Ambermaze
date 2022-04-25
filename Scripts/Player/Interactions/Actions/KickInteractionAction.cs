using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using LordAmbermaze.Interactions;
using UnityEngine;

namespace LordAmbermaze.Player
{
    public class KickInteractionAction : MonoBehaviour, IInteractionAction
    {
        public InteractibleType InteractType => InteractibleType.kickable;
        private AnimationManager anim;
        public void Interact(IInteractable interactable, Func callback)
        {
            anim.Play(AnimTypes.kick, () =>
            {
                interactable.Activate();
                callback?.Invoke();
            });
        }

        public void Init(Transform moveTransform)
        {
            anim = moveTransform.GetComponentInChildren<AnimationManager>();
        }
    }
}