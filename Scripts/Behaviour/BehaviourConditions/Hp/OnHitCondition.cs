using System;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public class OnHitCondition : MonoBehaviour, IBehaviourCondition
    {
        private IHaveDamageEvent _damageEventHolder;

        public event Func OnChangeState;
        private bool _active;
        public bool Active
        {
            get => _active;
            set
            {
                if (value != _active)
                {
                    OnChangeState?.Invoke();
                }

                _active = value;
            }
        }

        public void Init(Transform moveTransform)
        {
            _damageEventHolder = moveTransform.GetComponentInChildren<IHaveDamageEvent>();
            _damageEventHolder.OnHit += OnHit;
        }

        public void SetAgroGroup(IAgroGroup agroGroup)
        {
            throw new NotImplementedException();
        }

        public void ForceAgro()
        {
            throw new NotImplementedException();
        }

        private void OnEnable()
        {
            if (_damageEventHolder == null) return;
            _damageEventHolder.OnHit += OnHit;
        }

        private void OnDisable()
        {
            _damageEventHolder.OnHit -= OnHit;
        }

        private void OnHit()
        {
            _active = false;
            OnChangeState?.Invoke();
        }
    }
}