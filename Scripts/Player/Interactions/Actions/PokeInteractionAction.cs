using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using LordAmbermaze.Interactions;
using UnityEngine;

namespace LordAmbermaze.Player
{
    public class PokeInteractionAction : MonoBehaviour, IInteractionAction
    {
        public InteractibleType InteractType => InteractibleType.poke;
        private IMover _mover;
        private Transform _moveTransform;
        public void Interact(IInteractable interactable, Func callback)
        {
            var curPosition = (Vector2)_moveTransform.position;
            var direction = GetDirection(interactable);
            var delta = direction / 2;
            _mover.MoveTo(curPosition + delta, () =>
            {
                interactable.Activate();
                _mover.MoveTo(curPosition, () =>
                {
                    callback?.Invoke();
                }, 3);
            }, 3);
        }

        private Vector2 GetDirection(IInteractable interactable)
        {
            var interactPos = interactable.Pos;
            var pos = _moveTransform.position;
            if (Mathf.Abs(pos.x - interactPos.x) > 0.8f)
            {
                return pos.x > interactPos.x ? Vector2.left : Vector2.right;
            }
            else
            {
                return pos.y > interactPos.y ? Vector2.down : Vector2.up;
            }
        }

        public void Init(Transform moveTransform)
        {
            _mover = moveTransform.GetComponentInChildren<IMover>();
            _moveTransform = moveTransform;
        }
    }
}