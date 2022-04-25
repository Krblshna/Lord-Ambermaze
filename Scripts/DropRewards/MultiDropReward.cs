using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.DropRewards
{
    
    public class MultiDropReward : MonoBehaviour, IHaveOnDeathEvent, IDropReward
    {
        [SerializeField] private SimpleDrop[] _drops;
        public void OnDeathEvent()
        {
            CreateReward();
        }

        public void CreateReward()
        {
            if (_drops.Length == 0) return;
            DropsManager.Instance.CreateDrops(_drops, transform.position);
        }
    }
}