using AZ.Core;
using AZ.Core.Depot;
using LordAmbermaze.Core;
using LordAmbermaze.Player;
using TMPro;
using UnityEngine;

namespace LordAmbermaze.UI.Resources
{
    public class GoldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        private void Start()
        {
            UpdateGold();
            EventManager.StartListening($"resource:{ResType.Gold}", UpdateGold);
        }

        private void UpdateGold()
        {
            _textMesh.text = Storage.Instance.get(StorageType.Resources, ResType.Gold).ToString();
        }
    }
}