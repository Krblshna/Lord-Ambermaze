using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class SpiderBehaviourV2 : MoveAttackBehaviour
    {
        [SerializeField]
        private bool _prepared;

        public override void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            base.Init(moveTransform, characterEngine, tileChecker);
            _moveBehaviour.RecalcMoveDirection();
            var direction = MoveVector.Direction();
            _animManager.Turn(direction);
            characterEngine.SubscribeOnMoveFinish(_characterEngine.UrgentAttack);
            characterEngine.SubscribeOnMoveStart(() => _animManager.Play(AnimTypes.move));
            if (_prepared)
            {
                _animManager.Play(AnimTypes.prepare);
            }
        }
        public override void MakeMove()
		{
			if (MovesRemain < 1) return;
			_moveBehaviour.RecalcMoveDirection();
			var direction = MoveVector.Direction();
			//_animManager.Turn(direction);
			_tileChecker.UpdateChecker(direction);
            if (_prepared)
            {
                _prepared = false;
                if (_tileChecker.CouldAttack())
                {
                    Attack();
                }
                else
                {
                    TryMove();
                }
            }
            else
            {
                Prepare();
            }
		}

        private void TryMove()
        {
            if (_tileChecker.CouldMove())
            {
                var direction = MoveVector.Direction();
                _animManager.Turn(direction);
                Move();
            }
            else if (_tileChecker.ShouldConnect())
            {
                Connect();
            }
        }

        public override bool Wait()
        {
            _moveBehaviour.Wait();
            var direction = MoveVector.Direction();
            _tileChecker.UpdateChecker(direction);
            if (_tileChecker.CouldMove())
            {
                _animManager.Turn(direction);
                Move();
                return true;
            }
            else if (_tileChecker.ShouldConnect())
            {
                Connect();
                return true;
            } else 
            {
                Attack();
            }

            return false;
        }

        protected override void Attack(Func onAttack = null)
        {
            _animManager.Play(AnimTypes.move);
            _characterEngine.PrepareAttack(onAttack);
        }

        private void Prepare()
        {
            if (_tileChecker.HaveObstacle())
            {
                var direction = MoveVector.Direction();
                _animManager.Turn(direction * -1);
            }
            _animManager.Play(AnimTypes.prepare);
            _prepared = true;
            _characterEngine.Wait();
        }

        protected override void Move()
		{
            //if (_animManager != null)
            //{
            //    _animManager.Play(AnimTypes.move);
            //}
            _characterEngine.Move();
		}
    }
}