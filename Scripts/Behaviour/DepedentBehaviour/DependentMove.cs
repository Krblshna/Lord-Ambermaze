using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
    public class DependentMove : MonoBehaviour, IBehaviour, IDependencyBehaviour, IHaveOnDeathEvent
    {
        public GameObject ControlMonster;

        private ICharacterEngine _characterEngine;

        private ITileChecker _tileChecker;
        private IAnimManager _animManager;
        private ICharacterEngine _controlEngine;
        public Vector2 MoveVector { get; private set; }
        private int _maxMoves = 1;
        private bool _active = true, _dead = false;
        public int MovesRemain { get; private set; }

        public void SetControlMonster(GameObject monsterObj)
        {
            ControlMonster = monsterObj;
        }
        public void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
            _animManager = transform.GetComponentInChildren<IAnimManager>();
            _characterEngine = characterEngine;
            _tileChecker = tileChecker;
            _controlEngine = ControlMonster.GetComponent<ICharacterEngine>();
            _controlEngine.SubscribeOnMoveStart(OnMoveStart);
            //_controlMonster.GetComponent<>()
        }

        private void OnMoveStart()
        {
            if (MovesRemain < 1 || !_active || _dead) return;
            MoveVector = _controlEngine.MoveVector;
            _animManager.Turn(MoveVector);
            _tileChecker.UpdateChecker(MoveVector);
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
            else
            {
                _characterEngine.SpendMove(true);
            }
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

        public void MakeMove(){}

        public bool Wait()
        {
            return false;
            //throw new System.NotImplementedException();
        }

        public void OnMoveDone()
        {
            //throw new System.NotImplementedException();
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
            _active = active;
        }

        public void OnDeathEvent()
        {
            _dead = true;
        }
    }
}