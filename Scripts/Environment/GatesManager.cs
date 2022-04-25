using AZ.Core;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.Environment
{
    public class GatesManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gateBlock;
        private bool lastOpened;
        private bool GatesOpened => GameMaster.GatesOpened;
        private IGate[] _gates;
        private bool init;
        private void Awake()
        {
            Init();
            UpdateGates();
        }

        private void Init()
        {
            _gates = GetComponentsInChildren<IGate>();
            foreach (var gate in _gates)
            {
                gate.Init();
            }
        }

        private void UpdateGates()
        {
            if (!init)
            {
                init = true;
            }
            else if (GatesOpened != lastOpened)
            {
                SoundType soundType = GatesOpened ? SoundType.door_open : SoundType.door_close;
                SoundManager.PlaySound(soundType);
            }
            _gateBlock.SetActive(!GatesOpened);
            foreach (var gate in _gates)
            {
                gate.GateOpenChange(GatesOpened);
            }
            lastOpened = GatesOpened;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.StartListening("GatesChanged", UpdateGates);
        }

        private void UnsubscribeEvents()
        {
            EventManager.StopListening("GatesChanged", UpdateGates);
        }
    }
}