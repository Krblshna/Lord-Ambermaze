using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Monsters
{
    public class PushCollider : MonoBehaviour, IPushCollider
    {
        private IPushable _pushable;
        private void Awake()
        {
            _pushable = GetComponentInParent<IPushable>();
        }
        public void Push(Vector2Int pushDirection)
        {
            _pushable.Push(pushDirection);
        }
    }
}