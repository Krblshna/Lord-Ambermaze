using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Interactions;
using UnityEngine;

namespace LordAmbermaze.Player
{
    public class InteractionsManager : MonoBehaviour, IInteractionsManager
    {
        private Dictionary<InteractibleType, IInteractionAction> _actionsDict;
        public void Interact(IInteractable interactable, Func callback)
        {
            if (_actionsDict.TryGetValue(interactable.InteractType, out var interactionAction))
            {
                interactionAction.Interact(interactable, callback);
            }
        }

        public void Init(Transform moveTransform)
        {
            var interactionActions = GetComponentsInChildren<IInteractionAction>();
            foreach (var interactionAction in interactionActions)
            {
                interactionAction.Init(moveTransform);
            }
            _actionsDict = interactionActions.ToDictionary(interactionAction => interactionAction.InteractType,
                interactionAction => interactionAction);
        }
    }
}