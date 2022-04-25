using AZ.Core;
using LordAmbermaze.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LordAmbermaze.UI.Skills
{
    public class SkillUI : MonoBehaviour
    {
        [SerializeField] private string _counterName;
        [SerializeField] private GameObject _imageObj;
        [SerializeField] private GameObject _hotkey;
        [SerializeField] private Sprite active, inactive;
        private Image _image;
        private Condition _availableCondition;

        void Start()
        {
            _image = _imageObj.GetComponent<Image>();
            _availableCondition = new Condition(StorageType.Counters, _counterName, 1);
            CheckSkillAvailable();
            EventManager.StartListening("counters" + ":" + _counterName, CheckSkillAvailable);
            
        }

        void OnEnable()
        {
            PlayerState.SubscribeManaChange(CheckIsActive);
            EventManager.StartListening("counters" + ":" + _counterName, CheckSkillAvailable);
        }

        void OnDisable()
        {
            PlayerState.UnsubscribeManaChange(CheckIsActive);
            EventManager.StopListening("counters" + ":" + _counterName, CheckSkillAvailable);
        }

        void CheckSkillAvailable()
        {
            var isAvailable = _availableCondition.Test();
            _imageObj.SetActive(isAvailable);
            _hotkey.SetActive(isAvailable);
        }

        void CheckIsActive()
        {
            _image.sprite = PlayerState.CurrentMana > 0 ? active : inactive;
        }
    }
}