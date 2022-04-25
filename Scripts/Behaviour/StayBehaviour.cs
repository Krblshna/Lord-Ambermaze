using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class StayBehaviour : MonoBehaviour, IBehaviour
	{
		protected ICharacterEngine _characterEngine;
		protected ITileChecker _tileChecker;
		protected IMoveBehaviour _moveBehaviour;

		public Vector2 MoveVector { get; private set; }
		public int MovesRemain { get; private set; }
		private bool _wait = true;


		public void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
		{
			_characterEngine = characterEngine;
		}

		public virtual void MakeMove()
		{
		}

		protected virtual void Attack()
		{
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
