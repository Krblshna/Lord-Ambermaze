using System;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public abstract class HpCondition : MonoBehaviour, IBehaviourCondition
    {
        protected IHealth Health;

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
            Health = moveTransform.GetComponentInChildren<IHealth>();
            Health.OnChangeHp += OnChangeHp;
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
            if (Health == null) return;
            Health.OnChangeHp += OnChangeHp;
        }

        private void OnDisable()
        {
            if (Health != null)
            {
                Health.OnChangeHp -= OnChangeHp;
            }
        }

        private void OnChangeHp()
        {
            Debug.Log("Hp.Change");
            Active = ConditionCheck();
        }

        protected abstract bool ConditionCheck();
    }
}