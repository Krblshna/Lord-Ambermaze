using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Interactions
{
    public interface IInteractable
    {
        InteractibleType InteractType { get; }
        Vector2 Pos { get; }
        bool Available { get; }
        void Activate();
    }
}