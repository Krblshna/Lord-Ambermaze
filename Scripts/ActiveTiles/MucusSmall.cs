using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Sounds;
using UnityEngine;

namespace LordAmbermaze.ActiveTiles
{
    public class MucusSmall : MonoBehaviour
    {
        [SerializeField] private GameObject _active, _inactive;
        private bool _activated;
        private bool used;
        private IBattleManager _battleManager;

        private void Start()
        {
            EventManager.StartListening(EventList.TurnFinished, Check);
            _battleManager = transform.parent.parent.GetComponentInChildren<IBattleManager>();
            gameObject.SetActive(!_battleManager.IsSave());
        }

        private void Check()
        {
            if (_activated && !used)
            {
                used = true;
                SoundManager.PlaySound(SoundType.pink_punk);
                _active.SetActive(false);
                _inactive.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(Tags.Player))
            {
                OnEnter();
            }
        }

        private void OnEnter()
        {
            _activated = true;
        }
	}
}