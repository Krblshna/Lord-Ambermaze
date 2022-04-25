using UnityEngine;

namespace LordAmbermaze.ScenesManagement
{
    public class UndergroundTeleportCell : MonoBehaviour, IUndergroundCell
    {
        public Vector2 GetPos => transform.localPosition;
        [SerializeField] private Vector2Int _exitDirection;
        public Vector2Int ExitDirection => _exitDirection;
    }
}