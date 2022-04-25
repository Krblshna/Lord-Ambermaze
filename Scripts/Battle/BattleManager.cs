using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using AZ.Core;
using AZ.Core.Depot;
using LordAmbermaze.Core;
using LordAmbermaze.Level;
using LordAmbermaze.Player;
using LordAmbermaze.Projectiles;
using LordAmbermaze.TurnableObjects;
using UnityEngine;

namespace LordAmbermaze.Battle
{
	public class BattleManager : MonoBehaviour, IBattleManager
    {
        private static bool _firstTimeInit;
		[SerializeField, Range(0f, 5f)] private float timeSpeed;
        protected virtual bool _isTutor { get; }
		public IBlocker Blocker { get; private set; }
		public IBattleGround BattleGround { get; private set; }

        private Vector2 _playerNextPos;
		
		private List<IMonster> _monsters = new List<IMonster>();
        private List<ITurnableObject> _turnableObjects;
		private readonly List<IMonster> _needMoveMonsters = new List<IMonster>();
		private readonly List<IProjectile> _projectiles = new List<IProjectile>();
		private readonly List<IProjectile> _needMoveProjectiles = new List<IProjectile>();
        private IPreventGoNext[] _targets;

		public bool IsTurnActive { get; private set; }

		private void Awake()
		{
			Blocker = GetComponent<IBlocker>();
			BattleGround = GetComponent<IBattleGround>();
			
			var player = GetComponentInChildren<IBattlePlayer>();

			var playerCollider = player.Init(this);
			BattleGround.Init(playerCollider);

            var monsters = GetComponentsInChildren<IMonster>();
			foreach (var monster in monsters)
			{
				var active = monster.Init(this);
				if (active) _monsters.Add(monster);
			}

            _turnableObjects = GetComponentsInChildren<ITurnableObject>().ToList();
            foreach (var turnableObject in _turnableObjects)
            {
                turnableObject.Init(this);
            }

            _targets = GetComponentsInChildren<IPreventGoNext>();


			if (!_firstTimeInit && !_isTutor)
            {
                _firstTimeInit = true;
				CheckLevelComplete();
            }

            //var projectiles = GetComponentsInChildren<IProjectile>();
            //foreach (var projectile in projectiles)
            //{
            //	projectile.Init(this);
            //	_projectiles.Add(projectile);
            //}
        }

		IEnumerator Start()
		{
			BattleGround.SetTileData(_monsters);
			EventManager.StartListening(EventList.TargetCompleted, CheckLevelComplete);
			yield return new WaitForEndOfFrame();
            if (LevelManager.Instance == null)
            {
                CheckLevelComplete();
			}
		}

		public void LateUpdate()
		{
			UpdateGameSpeed();
			Move();
            if (IsTurnActive)
            {
                CheckTurnFinish();
            }
		}

        private void CheckTurnFinish()
        {
            if (Blocker.IsBlocked(Blocks.Attack)
                || Blocker.IsBlocked(Blocks.Movement)
                || Blocker.IsBlocked(Blocks.IntermediateMovement)) return;
            FinishTurn();
        }

        private void FinishTurn()
        {
            IsTurnActive = false;
			EventManager.TriggerEvent(EventList.TurnFinished);
        }

		private void UpdateGameSpeed()
		{
            //if (!Mathf.Approximately(Time.timeScale, timeSpeed)) Time.timeScale = timeSpeed;
        }

		public void PlayerNextPos(Vector2 destination)
        {
            _playerNextPos = destination;
			BattleGround.OnPlayerMove(destination);
            StartNewTurn();
        }

        private void StartNewTurn()
        {
            Prepare();
            Move();
            MoveTurnableObjects();
            AllMovementsStarted();
			EventManager.TriggerEvent(EventList.MoveStarted);
            IsTurnActive = true;
		}

		public void Register(IProjectile projectile)
		{
			_projectiles.Add(projectile);
		}

		void Prepare()
		{
			PrepareProjectiles();
			PrepareEnemies();
		}

		void Move()
		{
			MoveProjectiles();
			MoveEnemies();
		}

		private void PrepareProjectiles()
		{
			foreach (var projectile in _projectiles)
			{
				projectile.RefreshMoveSteps();
			}
			_needMoveProjectiles.AddRange(_projectiles);
		}

		private void PrepareEnemies()
		{
			foreach (var battleUnit in _monsters)
			{
				battleUnit.RefreshMoveSteps();
			}
			_needMoveMonsters.AddRange(_monsters);
		}

		private void MoveProjectiles()
		{
			if (_needMoveProjectiles.Count == 0) return;
			foreach (var projectile in _needMoveProjectiles)
			{
				projectile.MakeMove();
			}
			_needMoveProjectiles.Clear();
		}

        private void MoveTurnableObjects()
        {
            if (_turnableObjects.Count == 0) return;
            foreach (var turnableObject in _turnableObjects)
            {
                turnableObject.MakeMove();
            }
        }

		private void MoveEnemies()
		{
			if (_needMoveMonsters.Count == 0) return;
            var orderedBattleUnits =
                _needMoveMonsters.OrderByDescending(battleUnit => Vector2.Distance(battleUnit.CurPos, _playerNextPos));
			foreach (var battleUnit in orderedBattleUnits)
			{
				battleUnit.MakeMove();
			}
			_needMoveMonsters.Clear();
			Blocker.BlockedOrFireListeners(Blocks.Movement);
		}

		private void AllMovementsStarted()
		{
			foreach (var projectile in _projectiles)
			{
				projectile.AllMovementsStarted();
			}

            for (int i = 0; i < _monsters.Count; i++)
            {
                var battleUnit = _monsters[i];
				battleUnit.AllMovementsStarted();
			}
			//foreach (var battleUnit in _monsters)
			//{
			//	battleUnit.AllMovementsStarted();
			//}
		}

		public void Unregister(IProjectile projectile)
		{
			_projectiles.Remove(projectile);
		}

		public void Unregister(IMonster monster)
		{
			_monsters.Remove(monster);
            _needMoveMonsters.Remove(monster);
            CheckLevelComplete();
        }

        public void Register(IMonster monster)
        {
            monster.Init(this);
			_monsters.Add(monster);
            BattleGround.Register(monster);
		}

        public bool IsSave()
        {
            var count = _monsters.Count(monster => monster.ShouldBeKilled);
			return count == 0;
        }

        public bool AllTargetsCleared()
        {
            if (_targets.Length == 0) return true;
            return _targets.All(target => target.IsCleared());
        }

        public virtual void CheckLevelComplete()
        {
            var isSave = IsSave();
            var targetCleared = AllTargetsCleared();
            var levelComplete = isSave && targetCleared;
			if (levelComplete)
            {
				EventManager.TriggerEvent(EventList.RoomCleared);
                //Storage.Instance.Save("save0");
			}
			GameMaster.GatesOpened = levelComplete;
		}

		public void RequireMovement(IProjectile projectile, List<Vector2> curPoses, SlotType slotType)
		{
			_needMoveProjectiles.Add(projectile);
			BattleGround.RequireMovement(curPoses, slotType);
		}

		public void RequireMovement(IMonster battleMonster, List<Vector2> curPoses, SlotType slotType)
		{
			_needMoveMonsters.Add(battleMonster);
			BattleGround.RequireMovement(curPoses, slotType);
		}
	}
}