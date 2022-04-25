using System.Collections.Generic;
using UnityEngine;

namespace LordAmbermaze.Core
{
    public interface IColliderZone
    {
        HashSet<Vector2Int> Tiles { get; }
        void Init();
    }
}