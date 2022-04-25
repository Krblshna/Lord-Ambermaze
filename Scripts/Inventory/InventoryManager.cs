using AZ.Core;
using AZ.Core.Depot;
using UnityEngine;

namespace LordAmbermaze.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        private InventoryItem[] items;
        void Start()
        {
            Init();
            EventManager.StartListening(CommonEventList.ItemsChanged, UpdateUI);
            UpdateUI();
        }

        private void Init()
        {
            foreach (var item in items)
            {
                item.Init();
            }
        }

        private void UpdateUI()
        {
            foreach (var item in items)
            {
                var value = Storage.Instance.data.items.Get(item.Id);
                item.UpdateAmount(value);
            }
        }
    }
}