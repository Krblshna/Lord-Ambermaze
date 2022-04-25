using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AgroFlowerBehaviour : CommonAgroBehaviour, IHaveOnDeathEvent
    {
        private IBattleGround _battleGround;
        private AInteractableSlots _interactableSlots;
        private Vector2 _attackDirection;
        private bool _attackerPrepared;

        private List<Vector2> attackDirections = new List<Vector2>() { Vector2.left, Vector2.up, Vector2.right, Vector2.down };

        private Dictionary<Vector2, Vector2[]> attackDeltaCells = new Dictionary<Vector2, Vector2[]>()
        {
            {
                Vector2.left, new Vector2[]
                {
                    new Vector2(-1, -2), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1)
                }
            },
            {
                Vector2.up, new Vector2[]
                {
                    new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1)
                }
            },
            {
                Vector2.right, new Vector2[]
                {
                    new Vector2(2, -2), new Vector2(2, -1), new Vector2(2, 0), new Vector2(2, 1)
                }
            },
            {
                Vector2.down, new Vector2[]
                {
                    new Vector2(-1, -2), new Vector2(0, -2), new Vector2(1, -2), new Vector2(2, -2)
                }
            }
        };
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
            //else if (_tileChecker.CouldMove())
            //{
            //    Move(TryPrepare);
            //}
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
        }

        protected override void Attack()
        {
            _attackerPrepared = false;
            _characterEngine.PrepareAttack(() =>
            {
                CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
            });
        }

        public bool CouldAttack()
        {
            var r = new System.Random();
            foreach (var attackDirection in attackDirections.OrderBy(x => r.Next()))
            {
                var deltaCells = attackDeltaCells[attackDirection];
                if (deltaCells.Any(CouldAttack))
                {
                    _attackDirection = attackDirection;
                    return true;
                }
            }
            return false;
        }

        private void PrepareAttack()
        {
            _animManager.Play(AnimTypes.prepare);
            _moveDirection = _attackDirection;
            CellHighlightManager.Instance.SetCellsColor(GetInstanceID(), attackDeltaCells[_attackDirection].Select(GetAttackPos),
                CellHighlightType.Red);
            _attackerPrepared = true;
        }

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
            CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
        }
    }
}