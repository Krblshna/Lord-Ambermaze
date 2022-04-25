using AZ.Core;
using TMPro;
using UnityEngine;

namespace LordAmbermaze.Prices
{
    public class ItemPrice : MonoBehaviour, IItemPrice
    {
        private Price _price;
        private TextMeshPro textMesh;

        public void Init(Price price)
        {
            textMesh = GetComponentInChildren<TextMeshPro>();
            _price = price;
            textMesh.text = price.GetAmount().ToString();
        }
    }
}