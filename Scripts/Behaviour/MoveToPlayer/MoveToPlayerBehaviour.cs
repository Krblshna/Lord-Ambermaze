using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LordAmbermaze.Core;
using UnityEngine;
using AZ.Core;
using LordAmbermaze.AI;
using LordAmbermaze.Animations;

namespace LordAmbermaze.Behaviour
{
    public class MoveToPlayerBehaviour : MonoBehaviour, IBehaviour
    {
        protected ITileChecker _tileChecker;
        protected IAnimManager _animManager;
        private ICharacterEngine _characterEngine;
        private AIManager _aIManager;

        private Vector2 _moveDirection;
        private Transform _moveTransform;
        private IEnumerable<SlotType> _obstacleSlots;
        private int _maxMoves = 1;

        public Vector2 MoveVector => _moveDirection;
        public int MovesRemain { get; private set; }

        public void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            _tileChecker = tileChecker;
            _aIManager = transform.GetComponentInParent<AIManager>();
            _animManager = transform.GetComponentInChildren<IAnimManager>();
            _characterEngine = characterEngine;
            _moveTransform = moveTransform;
            _obstacleSlots = tileChecker.GetObstacleSlots();
        }

        public void MakeMove()
        {
            if (MovesRemain < 1) return;
            RecalcMoveDirection();
            var direction = MoveVector.Direction();
            _animManager.Turn(direction);
            _tileChecker.UpdateChecker(direction);
            if (_tileChecker.CouldMove())
            {
                Move();
            }
            else if (_tileChecker.CouldAttack())
            {
                Attack();
            }
            else if (_tileChecker.ShouldConnect())
            {
                Connect();
            }
            else if (HaveMaxMoves())
            {
                MetObstacle();
            }
            else
            {
                _characterEngine.SpendMove(true);
            }
        }

        protected bool HaveMaxMoves()
        {
            return MovesRemain == _maxMoves;
        }

        protected void MetObstacle()
        {
            _characterEngine.MetObstacle();
        }

        protected void Connect()
        {
            _characterEngine.TryConnect();
        }

        protected virtual void Attack()
        {
            _characterEngine.PrepareAttack();
        }

        protected void Move()
        {
            _animManager.Play(AnimTypes.move);
            _characterEngine.Move();
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
            MovesRemain = _maxMoves;
        }

        public void OnChangeState(bool active)
        {
        }

        private void RecalcMoveDirection()
        {
            _moveDirection = _aIManager.GetMoveDirectionToPlayer(_moveTransform, _obstacleSlots);
        }
    }
}