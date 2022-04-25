using UnityEngine;

namespace LordAmbermaze.ScenesManagement
{
    public interface IUndergroundCell
    {
        Vector2 GetPos { get; }
        Vector2Int ExitDirection { get; }
    }
}