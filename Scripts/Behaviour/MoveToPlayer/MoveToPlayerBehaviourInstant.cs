using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LordAmbermaze.Core;
using UnityEngine;
using AZ.Core;
using LordAmbermaze.AI;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;

namespace LordAmbermaze.Behaviour
{
    public class MoveToPlayerBehaviourInstant : MonoBehaviour, IBehaviour
    {

        private Transform _moveTransform;
        private IMover _mover;
        private IBattleGround _battleGround;
        public Vector2 MoveVector { get; }
        public int MovesRemain { get; private set; }

        public void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            _moveTransform = moveTransform;
            _battleGround = moveTransform.GetComponentInParent<BattleGround>();
            _mover = moveTransform.GetComponentInChildren<IMover>();
        }

        public void MakeMove()
        {
            _mover.MoveTo(_battleGround.PlayerNextPos, OnMoved, 1);
        }

        private void OnMoved()
        {

        }
        
        public bool Wait()
        {
            return false;
        }

        public void OnMoveDone()
        {
        }

        
        public void SpendMove(bool all)
        {
            var steps = all ? MovesRemain : 1;
            MovesRemain -= steps;
        }

        public void RefreshMoveSteps()
        {
        }

        public void OnChangeState(bool active)
        {
        }

        private void RecalcMoveDirection()
        {
        }
    }
}