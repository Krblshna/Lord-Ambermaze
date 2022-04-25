using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using LordAmbermaze.Core;
using UnityEngine;
using AZ.Core;
using LordAmbermaze.AI;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.CellHighlight;
using LordAmbermaze.InteractableSlots;
using Vector2 = UnityEngine.Vector2;

namespace LordAmbermaze.Behaviour
{
    public class AgroBeeBehaviour : CommonAgroBehaviour, IHaveOnDeathEvent
    {
        private IBattleGround _battleGround;
        private AInteractableSlots _interactableSlots;
        private Vector2 _attackDirection;
        private bool _attackerPrepared, _dead;
        public override void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            base.Init(moveTransform, characterEngine, tileChecker);
            _battleGround = moveTransform.GetComponentInParent<IBattleGround>();
            _interactableSlots = tileChecker.InteractableSlots;
        }
        public override void MakeMove()
        {
            if (MovesRemain < 1) return;
            if (_attackerPrepared)
            {
                _tileChecker.UpdateChecker(_attackDirection);
                Attack();
                return;
            }
            RecalcMoveDirection();
            var direction = MoveVector.Direction();
            _animManager.Turn(direction);
            _tileChecker.UpdateChecker(direction);

            if (CouldAttack())
            {
                PrepareAttack();
            }
            else if (CouldLongAttack())
            {
                PrepareAttack();
            }
            else if (_tileChecker.CouldMove())
            {
                Move(TryPrepare);
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

        public override void OnChangeState(bool active)
        {
            if (!active) return;
            TryPrepare();
        }

        private void TryPrepare()
        {
            RecalcMoveDirection();
            var direction = MoveVector.Direction();
            _animManager.Turn(direction);
            _tileChecker.UpdateChecker(direction);

            if (CouldAttack())
            {
                PrepareAttack();
            }
            else if (CouldLongAttack())
            {
                PrepareAttack();
            }
        }

        protected override void Attack()
        {
            _attackerPrepared = false;
            
            if (_tileChecker.CouldMove())
            {
                Move(() => _characterEngine.Attack(() =>
                {
                    CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
                }));
            }
            else
            {
                _characterEngine.PrepareAttack(() =>
                {
                    CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
                });
            }
        }

        public bool CouldAttack()
        {
            var direction = MoveVector.Direction();
            return CouldAttack(direction);
        }

        public bool CouldLongAttack()
        {
            var direction = MoveVector.Direction();
            return _tileChecker.CouldMove() && CouldAttack(direction * 2);
        }

        private void PrepareAttack()
        {
            _attackDirection = MoveVector.Direction();
            _animManager.SetDirection(_attackDirection);
            _animManager.Play(AnimTypes.prepare);
            if (!_dead)
            {
                CellHighlightManager.Instance.SetCellsColor(GetInstanceID(),
                    new Vector2[] {GetAttackPos(_attackDirection), GetAttackPos(_attackDirection * 2)},
                    CellHighlightType.Red);
            }

            _attackerPrepared = true;
        }

        //public bool CouldLongAttack()
        //{
        //    var direction = MoveVector.Direction();
        //    var checkTile = (Vector2)_moveTransform.position + direction * 2;
        //    var tileData = _battleGround.GetTileData(checkTile);
        //    return _tileChecker.CouldMove() && _interactableSlots.HaveAttackableSlot(tileData.slotType);
        //}

        private Vector2 GetAttackPos(Vector2 delta)
        {
            return (Vector2)_moveTransform.position + delta;
        }

        private bool CouldAttack(Vector2 delta)
        {
            var checkTile = GetAttackPos(delta);
            var slotType = _battleGround.CheckNextMovePos(checkTile);
            return _interactableSlots.HaveAttackableSlot(slotType);
        }

        private void RecalcMoveDirection()
        {
            _moveDirection = _aIManager.GetMoveDirectionToPlayer(_moveTransform, _obstacleSlots);
        }

        public void OnDeathEvent()
        {
            _dead = true;
            CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
        }
    }
}