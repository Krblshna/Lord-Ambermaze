using System;
using AZ.Core;
using LordAmbermaze.Interactions;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.Environment
{
    public class Gate : MonoBehaviour, IGate
    {
        [SerializeField] private GameObject portalObj;
        private Animator _anim;
        private static readonly int Open = Animator.StringToHash("open");
        private bool _active;
        private DoorLock _doorLock;
        private bool LockOpen => _doorLock == null || _doorLock.Opened;

        public void Init()
        {
            _anim = GetComponent<Animator>();
            portalObj.SetActive(false);
        }

        public void SetLock(DoorLock doorLock)
        {
            _doorLock = doorLock;
        }

        public void OnLockOpen()
        {
            CheckGateOpen();
        }

        private void CheckGateOpen()
        {
            var active = LockOpen && _active;
            _anim.SetBool(Open, active);
            portalObj.SetActive(active);
        }

        public void GateOpenChange(bool active)
        {
            _active = active;
            CheckGateOpen();
        }
    }
}