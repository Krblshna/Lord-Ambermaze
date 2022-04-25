using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Inventory;
using UnityEngine;

namespace LordAmbermaze.Prices
{
	[System.Serializable]
	public class PriceInventory
    {
		[SerializeField] private ItemType _itemType;
        [SerializeField] private int _amount;
        private Price _price;
        public Price price
        {
            get
            {
                if (_price == null)
                {
                    Init();
                }

                return _price;
            }
        }

        private void Init()
        {
            var key = ((int)_itemType).ToString();
            _price = new Price(StorageType.Inventory, key, _amount);
		}
	}
}
