using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.Behaviour;
using LordAmbermaze.Core;
using LordAmbermaze.InteractableSlots;
using UnityEngine.Events;

namespace LordAmbermaze.Monsters
{
	public class CharacterEngine : MonoBehaviour, ICharacterEngine, IMoveConnecteable
    {
        [SerializeField] private UnityEvent _onMove, _onMoveStarted;
		private IMover _mover;
		private IBlocker _blocker;
		private IAttacker _attacker;
		private IBodyTiles _bodyTiles;
		private IBehaviour _behaviour;
		private IMoveConnector _moveConnector;
		private ITileChecker _tileChecker;
		private ICanLongMove _longMover;
		private IBattleGround _battleGround;
		private IAnimManager _animManager;
        private ICellEffector _cellEffector;

		private SlotType _slotType;
		private Transform _moveTransform;
		private readonly List<TileData> _ownTilesData = new List<TileData>();

		private int Speed => MoveVector.IntLength();
		public Vector2 MoveVector => _behaviour.MoveVector;
		private int MovesRemain => _behaviour.MovesRemain;
        private bool _pushed;
		private Func _customOnMoveFinish, _customOnMoveStart;

        public void SubscribeOnMoveFinish(Func onMove)
        {
            _customOnMoveFinish += onMove;
        }

        public void SubscribeOnMoveStart(Func onMove)
        {
            _customOnMoveStart += onMove;
        }

		public bool MoveMade {
			get => _moveConnector.MoveMade;
			set => _moveConnector.MoveMade = value;
		}

        private bool _isDead;
        private Blocks _lastBlock;
		
		public void Init(IBattleManager battleManager, Transform moveTransform, AInteractableSlots interactableSlots)
		{
			_behaviour = GetComponent<IBehaviour>();
			_tileChecker = GetComponentInChildren<ITileChecker>();
			_mover = GetComponentInChildren<IMover>();
			_attacker = GetComponentInChildren<IAttacker>();
			_moveConnector = GetComponentInChildren<IMoveConnector>();
			_bodyTiles = GetComponentInChildren<IBodyTiles>();
			_longMover = moveTransform.GetComponent<ICanLongMove>();
			_animManager = moveTransform.GetComponentInChildren<IAnimManager>();
            _cellEffector = moveTransform.GetComponentInChildren<ICellEffector>();
			var coll = moveTransform.GetComponentInChildren<ICharacterCollider>();

			_moveConnector.Init(this, coll);
			_mover.Init(moveTransform);
			_attacker.Init(battleManager, moveTransform);
			_tileChecker.Init(_bodyTiles, battleManager, interactableSlots);
			_behaviour.Init(moveTransform, this, _tileChecker);

			_slotType = interactableSlots.SlotType;
			_moveTransform = moveTransform;
			_battleGround = battleManager.BattleGround;
			_blocker = battleManager.Blocker;

			UpdateTileData();
		}

		public void MakeMove()
        {
            if (_pushed) return;
			_behaviour.MakeMove();
		}

		public void TryConnect()
		{
			var tilesData = _tileChecker.FilteredCurrentCheckTilesData(tileData =>
				tileData.moveConnector != null && !tileData.moveConnector.MoveMade);

			foreach (var tileData in tilesData)
			{
				if (tileData.moveConnector != null && !tileData.moveConnector.MoveMade)
					_moveConnector.ConnectTo(tileData.moveConnector);
			}
        }

		protected virtual void AddToBattleGround(Vector2 direction)
		{
			var moveCells = _bodyTiles.GetMoveCells(direction);
			_battleGround.MoveTo(moveCells, _slotType);
		}

		public void MetObstacle()
        {
            var haveMove = _behaviour.Wait();
            if (!haveMove)
            {
                SpendMove(true);
                AddToBattleGround(Vector2.zero);
                OnMoveMade(false);
            }
        }

		public void PrepareAttack(Func onAttackCallback = null)
		{
			SpendMove(true);
			_blocker.StartListeningUnblock(Blocks.Movement, () => Attack(onAttackCallback));
			AddToBattleGround(Vector2.zero);
			OnMoveMade(false);
		}

		public void Move(Func onMoveFinishCallback = null)
		{
            if (_isDead) return;
			_onMoveStarted?.Invoke();
            _customOnMoveStart?.Invoke();
			var direction = MoveVector.Direction();
			var newPos = direction + (Vector2)_moveTransform.position;
			SpendMove();
            _lastBlock = Speed > 1 ? Blocks.IntermediateMovement : Blocks.Movement;
			var callback = Speed > 1 ? (Func)OnIntermediateMoveFinish : (Func)OnMoveFinish;
            if (onMoveFinishCallback != null)
            {
                callback += onMoveFinishCallback;
            }

            if (_customOnMoveFinish != null)
            {
                callback += _customOnMoveFinish;
            }

            _blocker.Block(_lastBlock);
			_mover.MoveTo(newPos, callback, Speed);
			AddToBattleGround(direction);
			_behaviour.OnMoveDone();
			OnMoveMade(true);
		}

		public void Attack(Func onAttackCallback = null)
        {
            if (_isDead) return;
			//_blocker.Block(Blocks.Movement);
			_attacker.Attack(MoveVector.Direction(), onAttackCallback);
			//_animManager.Play(AnimTypes.attack, () =>
			//{
			//	_blocker.Unblock(Blocks.Movement);
			//});
		}

        public void Wait()
        {
            AddToBattleGround(Vector2.zero);
			SpendMove(true);
            OnMoveMade(true);
		}

		public void UrgentAttack()
		{
			SpendMove(true);
			AddToBattleGround(Vector2.zero);
			Attack();
			OnMoveMade(false);
		}

		public void CustomDestroy()
		{
			var oldTilesData = _ownTilesData.ToList();
			_battleGround.Unregister(oldTilesData);
		}

        public void Death()
        {
            _isDead = true;
			_mover.Stop();
        }

        public void Push(Vector2Int pushDirection)
        {
			var direction = pushDirection;
            var newPos = direction + (Vector2)_moveTransform.position;
            SpendMove(true);
            _blocker.Block(Blocks.Movement);
			_mover.MoveTo(newPos, () =>
            {
                OnMoveFinish();
                _cellEffector.Check();
			}, pushDirection.IntLength());
            AddToBattleGround(direction);
            _behaviour.OnMoveDone();
            OnMoveMade(true);
            _pushed = true;
        }

        public void AllMovementsStarted()
        {
            _pushed = false;
			if (MoveMade) return;
			var cycleConnection = _moveConnector.CheckCycleConnection();
			var closeCycleConnection = _moveConnector.HaveCloseCycleConnection();
			if (cycleConnection && !closeCycleConnection && _moveConnector.CouldCycleMove())
			{
				OnConnectionMove(true, true);
			}
			else
			{
				MetObstacle();
			}
		}

		private void OnMoveFinish()
		{
            _onMove?.Invoke();
			OnChangePosition();
			_blocker.Unblock(_lastBlock);
            CheckCellEffect();
        }

        private void CheckCellEffect()
        {
            var groundTile = _battleGround.GetGroundTileData(transform.position);
		}

		private void OnIntermediateMoveFinish()
		{
			//FIXME
			if (MovesRemain > 0 && gameObject.activeInHierarchy && !_isDead)
			{
				var moveCells = _bodyTiles.GetMoveCells(Vector2.zero);
				_longMover.RequireMovement(moveCells);
				MoveMade = false;
			}
			OnChangePosition();
			_blocker.Unblock(_lastBlock);
		}

		protected virtual void OnChangePosition()
		{
			var oldTilesData = _ownTilesData.ToList();
			var newTilesData = GetUpdatedTilesData();
			_battleGround.UpdateTileData(oldTilesData, newTilesData);
		}

		public void SpendMove(bool all=false)
		{
			_behaviour.SpendMove(all);
		}

		public void RefreshMoveSteps()
		{
			MoveMade = false;
			_behaviour.RefreshMoveSteps();
		}

		private void UpdateTileData()
		{
			_ownTilesData.Clear();
			var moveCells = _bodyTiles.GetMoveCells(Vector2.zero);
			foreach (var moveCell in moveCells)
			{
				_ownTilesData.Add(new TileData(moveCell, _moveConnector, _slotType));
			}
		}

		public List<TileData> GetUpdatedTilesData()
		{
			UpdateTileData();
			return _ownTilesData;
		}

		public void OnMoveMade(bool moved)
		{
			MoveMade = true;
			_moveConnector.OnMoveMade(moved);
		}

        public void OnConnectionMove(bool moved)
        {
            OnConnectionMove(moved, false);
        }

		public void OnConnectionMove(bool moved, bool force=false)
		{
			if (!moved || MoveMade) return;
			_tileChecker.UpdateChecker(MoveVector.Direction());
			if (_tileChecker.CouldMove() || force)
			{
				Move();
			}
			else
			{
				MetObstacle();
			}
		}

		public int RegisterNextMove(HashSet<string> moveSet)
		{
			var moveCells = _bodyTiles.GetMoveCells(MoveVector.Direction());
			foreach (var moveCell in moveCells)
			{
				string coordStr = CoordinateUtils.PosToCoord(moveCell).ToString();
				moveSet.Add(coordStr);
			}

			return moveCells.Count;
		}
	}
}