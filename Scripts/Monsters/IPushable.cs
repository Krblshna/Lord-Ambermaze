using UnityEngine;

namespace LordAmbermaze.Monsters
{
    public interface IPushable
    {
        void Push(Vector2Int pushDirection);
    }
}