using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Effects;
using UnityEngine;

namespace LordAmbermaze.ActiveTiles
{
    public class MucusSlime : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _existTurns = 2;
        private bool _activated;
        private ICharacterCollider _charCollider;
        private IDamageHandler _damageHandler;

        private void Start()
        {
            _damageHandler = GetComponentInChildren<IDamageHandler>();
            EventManager.StartListening(EventList.TurnFinished, Check);
            EventManager.StartListening(EventList.MoveStarted, MoveStarted);
        }

        private void MoveStarted()
        {
            _existTurns -= 1;
            if (_existTurns <= 0)
            {
                _damageHandler.Death(() =>
                {
                    Destroy(gameObject);
                });
            }
        }

        private void Disappear()
        {

        }

        private void Check()
        {
            if (_activated)
            {
                _charCollider?.Hit(_damage);
                EffectsManager.Instance.CreateEffect(EffectType.poison, transform.position);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(Tags.Player))
            {
                _charCollider = collider.GetComponent<ICharacterCollider>();
                _activated = true;
            }
        }
    }
}