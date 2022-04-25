using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class MoveAttackBehaviour : MonoBehaviour, IBehaviour
	{
		protected ICharacterEngine _characterEngine;

		protected ITileChecker _tileChecker;
		protected IMoveBehaviour _moveBehaviour;
		protected IAnimManager _animManager;

		private int _maxMoves;

		public Vector2 MoveVector => _moveBehaviour.GetMoveVector();
		public int MovesRemain { get; private set; }

		public virtual void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
		{
			_moveBehaviour = GetComponent<IMoveBehaviour>();
			_animManager = transform.GetComponentInChildren<IAnimManager>();
            _characterEngine = characterEngine;
			_tileChecker = tileChecker;

            _moveBehaviour.Init(moveTransform, tileChecker.GetObstacleSlots());
			RefreshMoveSteps();
			_maxMoves = MovesRemain;
		}

		public virtual void MakeMove()
		{
			if (MovesRemain < 1) return;
			_moveBehaviour.RecalcMoveDirection();
			var direction = MoveVector.Direction();
			_animManager.Turn(direction);
			_tileChecker.UpdateChecker(direction);
			if (_tileChecker.CouldAttack())
			{
				Attack();
			}
            else if (_tileChecker.CouldMove())
            {
                Move();
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

		protected void MetObstacle()
		{
			_characterEngine.MetObstacle();
		}

		protected void Connect()
		{
			_characterEngine.TryConnect();
		}

		protected virtual void Attack(Func onAttack = null)
		{
			_characterEngine.PrepareAttack(onAttack);
		}

		protected virtual void Move()
		{
            if (_animManager != null)
            {
                _animManager.Play(AnimTypes.move);
            }
            _characterEngine.Move();
		}

		public virtual bool Wait()
		{
			_moveBehaviour.Wait();
            return false;
        }

		public void OnMoveDone()
		{
			_moveBehaviour.OnMoveDone();
		}

		protected bool HaveMaxMoves()
		{
			return MovesRemain == _maxMoves;
		}

		public void SpendMove(bool all = false)
		{
			var steps = all ? MovesRemain : 1;
			MovesRemain -= steps;
		}

		public void RefreshMoveSteps()
		{
			MovesRemain = _moveBehaviour.GetMaxMoves();
		}

        public void OnChangeState(bool active)
        {
            _moveBehaviour.OnChangeState(active);
        }
    }
}