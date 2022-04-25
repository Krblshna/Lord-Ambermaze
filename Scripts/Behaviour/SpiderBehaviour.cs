using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class SpiderBehaviour : MoveAttackBehaviour
    {
        private bool _prepared;

        public override void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            base.Init(moveTransform, characterEngine, tileChecker);
            _moveBehaviour.RecalcMoveDirection();
            var direction = MoveVector.Direction();
            _animManager.Turn(direction);
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
                Attack(TryMove);
            }
            else
            {
                Prepare();
            }
		}

        private void TryMove()
        {
            _prepared = false;
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
            else
            {
                _moveBehaviour.Wait();
                var direction = MoveVector.Direction();
                _animManager.Turn(direction);
                _tileChecker.UpdateChecker(direction);
                if (_tileChecker.CouldMove())
                {
                    Move();
                }
                else if (_tileChecker.ShouldConnect())
                {
                    Connect();
                }
            }
        }

        private void Prepare()
        {
            if (!_tileChecker.CouldMove())
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
            if (_animManager != null)
            {
                _animManager.Play(AnimTypes.move);
            }
            _characterEngine.Move();
		}
    }
}