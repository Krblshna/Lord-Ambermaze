using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Attackers;
using LordAmbermaze.CellHighlight;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class BlowBehaviour : MonoBehaviour, IBehaviour, IHaveOnDeathEvent
	{
		protected ICharacterEngine _characterEngine;
		protected ITileChecker _tileChecker;
		protected IMoveBehaviour _moveBehaviour;
        private IBlocker _blocker;
        private IAreaAttacker _attacker;

        [SerializeField]
        private EMoveDirection[] _fireDirections;

        private bool initialized;
        private ICollection<Vector2> _attackCells;
        private ICollection<Vector2> attackCells
        {
            get
            {
                if (!initialized)
                {
                    initialized = true;
                    _attackCells = _fireDirections.Select(direction => Utils.DirectionToVector(direction) + (Vector2)transform.position).ToList();
                }

                return _attackCells;
            }
        }

        public Vector2 MoveVector { get; private set; }
		public int MovesRemain { get; private set; }
        [SerializeField]
		private int _waitTurns = 2;

        private void Init()
        {
            _attackCells = _fireDirections.Select(direction => Utils.DirectionToVector(direction) + (Vector2)transform.position).ToList();
        }

		public void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
		{
			_characterEngine = characterEngine;
            _blocker = moveTransform.GetComponentInParent<IBlocker>();
        }

		public virtual void MakeMove()
        {
            _waitTurns -= 1;
			if (_waitTurns > 0) return;
			Attack();
		}

		protected virtual void Attack()
		{
            _blocker.StartListeningUnblock(Blocks.Movement, _characterEngine.UrgentAttack);
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

        private void OnDisable()
        {
            CellHighlightManager.Instance.RemoveHighlightCells(GetInstanceID());
        }

        public void OnChangeState(bool active)
        {
            if (active)
            {
                CellHighlightManager.Instance.PermanentBlinkCells(GetInstanceID(), attackCells, CellHighlightType.Red);
			}
        }

        public void OnDeathEvent()
        {
            CellHighlightManager.Instance.RemoveHighlightCells(GetInstanceID());
		}
	}
}
