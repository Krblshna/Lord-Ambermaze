using TMPro;
using UnityEngine;

namespace LordAmbermaze.Inventory
{
    [System.Serializable]
    public class InventoryItem
    {
        public int Id;
        private int _amount;
        [SerializeField]
        private GameObject itemObj;
        private TextMeshProUGUI counterText;
        private GameObject textObj;

        public void Init()
        {
            counterText = itemObj.GetComponentInChildren<TextMeshProUGUI>();
            textObj = counterText.gameObject;
        }

        public void UpdateAmount(int value)
        {
            _amount = value;
            OnAmountChange();
        }

        private void OnAmountChange()
        {
            counterText.text = _amount.ToString();
            textObj.SetActive(_amount > 1);
            itemObj.SetActive(_amount > 0);
        }
    }
}