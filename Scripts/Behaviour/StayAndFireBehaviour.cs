using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class StayAndFireBehaviour : MonoBehaviour, IBehaviour
	{
		[SerializeField]
		private EMoveDirection _direction;

		protected ICharacterEngine _characterEngine;
		protected ITileChecker _tileChecker;
		protected IMoveBehaviour _moveBehaviour;

		public Vector2 MoveVector { get; private set; }
		public int MovesRemain { get; private set; }
		[SerializeField] int _waitTurns = 1;
        private int _waitedTurns;

		private void Awake()
		{
			MoveVector = Utils.DirectionToVector(_direction);
		}

		public void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
		{
			_characterEngine = characterEngine;
            _waitedTurns = _waitTurns;
        }

		public virtual void MakeMove()
		{
            if (_waitedTurns < _waitTurns)
            {
                _waitedTurns += 1;
                return;
			}

            _waitedTurns = 0;
			Attack();
		}

		protected virtual void Attack()
		{
			_characterEngine.UrgentAttack();
		}

		protected void Move()
		{
		}

		public bool Wait()
		{
            return false;
		}

		public void OnMoveDone()
		{
		}

		public void SpendMove(bool all = false)
		{
		}

		public void RefreshMoveSteps()
		{
		}

        public void OnChangeState(bool active)
        {
            
        }
    }
}
