using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Teleport
{
    public class TeleportCell : MonoBehaviour, ITeleportCell
    {
        private IBattleManager _battleManager;
        private Animator _anim;
        private bool IsSave => _battleManager.IsSave();
        public Vector2 GetPos => transform.localPosition;
        private bool _active;
        private void Start()
        {
            _battleManager = GetComponentInParent<IBattleManager>();
            _anim = GetComponentInChildren<Animator>();
            _anim.SetBool("visible", IsSave);
            _anim.enabled = true;
            EventManager.StartListening(EventList.RoomCleared, SetPortalState);
        }

        public void SetPortalState()
        {
            _anim.SetBool("show", _active);
            _anim.SetBool("visible", IsSave);
            EventManager.TriggerEvent(_active ? EventList.TeleportEnter : EventList.TeleportExit);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.tag.Equals(Tags.Player)) return;
            _active = true;
            if (!IsSave) return;
            SetPortalState();
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (!collider.tag.Equals(Tags.Player)) return;
            _active = false;
            if (!IsSave) return;
            SetPortalState();
        }

    }
}