using System;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.UI.Misc
{
    public class ScreenHider : MonoBehaviour
    {
        private Animator _anim;
        private static readonly int Hide = Animator.StringToHash("hide");
        private static readonly int Show = Animator.StringToHash("show");

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }
        private void Start()
        {
            EventManager.StartListening(EventList.ScreenHide, ScreenHide);
            EventManager.StartListening(EventList.ScreenShow, ScreenShow);
        }

        private void ScreenHide()
        {
            _anim.SetTrigger(Hide);
        }

        private void ScreenShow()
        {
            _anim.SetTrigger(Show);
        }

        public void OnShow()
        {
            EventManager.TriggerEvent(EventList.OnScreenShowed);
        }

        public void OnHide()
        {
            EventManager.TriggerEvent(EventList.OnScreenHided);
        }
    }
}