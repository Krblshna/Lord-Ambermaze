using UnityEngine;
namespace LordAmbermaze.Teleport
{
    public interface ITeleportCell
    {
        Vector2 GetPos { get; }
    }
}