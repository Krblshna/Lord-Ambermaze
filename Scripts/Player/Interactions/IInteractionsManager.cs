using AZ.Core;
using LordAmbermaze.Interactions;
using UnityEngine;

namespace LordAmbermaze.Player
{
    public interface IInteractionsManager
    {
        void Interact(IInteractable interactable, Func callback);
        void Init(Transform moveTransform);
    }
}