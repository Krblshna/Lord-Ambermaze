using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.DropRewards
{
    public class DropReward : MonoBehaviour, IHaveOnDeathEvent
    {
        [SerializeField] private DropType _dropType;
        [SerializeField] private float _dropChance = 0.3f;
        public void OnDeathEvent()
        {
            if (_dropType == DropType.none) return;
            if (!Utils.SimpleChance(_dropChance)) return;
            DropsManager.Instance.CreateDrop(DropType.apple, transform.position);
        }
    }
}