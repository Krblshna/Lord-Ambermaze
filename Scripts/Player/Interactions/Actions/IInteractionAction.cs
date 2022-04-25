using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Interactions;
using UnityEngine;

namespace LordAmbermaze.Player
{
    public interface IInteractionAction
    {
        InteractibleType InteractType { get; }
        void Interact(IInteractable interactable, Func callback);
        void Init(Transform moveTransform);
    }
}