using UnityEngine;

namespace LordAmbermaze.Core
{
    public interface IPushCollider
    {
        void Push(Vector2Int pushDirection);
    }
}