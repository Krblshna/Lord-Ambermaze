using System;
using System.Collections.Generic;
using AZ.Core;
using AZ.Core.Conditions;
using LordAmbermaze.AI;
using LordAmbermaze.Battle;
using LordAmbermaze.CellHighlight;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public abstract class PlayerDistanceCondition : MonoBehaviour, IBehaviourCondition
    {
        [SerializeField] protected comparators _comparator;
        [SerializeField] protected bool _permaAgro = true;
        protected AIManager _aImanager;
        protected Transform _moveTransform;
        protected List<Vector2> _zoneCells = new List<Vector2>();
        protected readonly List<Vector2> _zoneCellsOffset = new List<Vector2>();
        private IBlocker _blocker;
        private IAgroGroup _agroGroup;
        public event Func OnChangeState;
        private bool _activated;
        private bool _active;
        public bool Active
        {
            get => _active || _activated;
            set
            {
                var prevActive = _active;
                _active = value;
                if (value && !_activated)
                {
                    if (_permaAgro)
                    {
                        _activated = true;
                    }

                    if (!prevActive)
                    {
                        ShowZone(value);
                        _agroGroup?.OnAgro();
                        OnChangeState?.Invoke();
                    }
                }
            }
        }

        private void Awake()
        {
            SetInitCells();
        }

        private void ShowZone(bool active)
        {
            if (!active) return;
            UpdateZone();
            var color = CellHighlightType.Yellow;
            CellHighlightManager.Instance.HighlightCells(_zoneCells, color);
        }

        void Start()
        {
            OnPlayerMovementFinish();
        }

        public virtual void Init(Transform moveTransform)
        {
            _moveTransform = moveTransform;
            _aImanager = moveTransform.GetComponentInParent<AIManager>();
            var battleManager = moveTransform.GetComponentInParent<BattleManager>();
            _blocker = battleManager.Blocker;
            EventManager.StartListening(EventList.TurnFinished, OnPlayerMovementFinish);
        }

        public void SetAgroGroup(IAgroGroup agroGroup)
        {
            _agroGroup = agroGroup;
        }

        public void ForceAgro()
        {
            Active = true;
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventList.TurnFinished, OnPlayerMovementFinish);
            _agroGroup?.Unregister(this);
        }

        private void OnPlayerMovementFinish()
        {
            //
            Active = ConditionCheck();
        }

        protected abstract bool ConditionCheck();
        protected abstract void SetInitCells();
        protected virtual void UpdateZone()
        {
            _zoneCells.Clear();
            var monsterPos = _moveTransform.position;
            foreach (var zoneOffset in _zoneCellsOffset)
            {
                _zoneCells.Add(zoneOffset + (Vector2)monsterPos);
            }
        }
    }
}