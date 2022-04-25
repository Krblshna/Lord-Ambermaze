using System.Collections;
using System.Collections.Generic;
using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.ScenesManagement
{
    public class DemoPage : MonoBehaviour
    {
        private bool _controlable = false;
        private IEnumerator _process_en;
        void Start()
        {
            _process_en = Process();
            CommonBlocker.Block(CommonBlocks.Comics);
            StartCoroutine(_process_en);
        }

        IEnumerator Process()
        {
            yield return new WaitForSeconds(2);
            _controlable = true;
            yield return new WaitForSeconds(10);
            ClosePage();
        }

        void Update()
        {
            if (ButtonWrap.GetButtonDown(CommonButtons.Use) || ButtonWrap.GetButtonDown(CommonButtons.Jump))
            {
                ClosePage();
            }
        }

        private void ClosePage()
        {
            if (_process_en != null)
            {
                StopCoroutine(_process_en);
            }
            _process_en = null;
            _controlable = false;
            CommonBlocker.Unblock(CommonBlocks.Comics);
            ScenesManager.FinishGame();
        }
    }
}