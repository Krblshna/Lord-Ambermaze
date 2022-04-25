using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Battle;
using LordAmbermaze.CellHighlight;
using LordAmbermaze.Core;
using LordAmbermaze.Monsters;
using UnityEngine;

namespace LordAmbermaze.Behaviour
{
	public class FlowerMoveBehaviour : MoveAttackBehaviour, IHaveOnDeathEvent
    {
        public GameObject _minionPreviewPrefab;
        public GameObject _minionPrefab;
        private List<GameObject> _minionPreviews = new List<GameObject>();
        private IBattleGround _battleGround;
        private IBattleManager _battleManager;
        private Transform _moveTransform;
        private Vector2[] _deltaPoses = new Vector2[] {new Vector2(-1, 1), new Vector2(-1, -2), new Vector2(2, 1), new Vector2(2, -2) };
        private bool _dead;
        private IEnumerable<Vector2> _activeCells;

		public override void Init(Transform moveTransform, ICharacterEngine characterEngine, ITileChecker tileChecker)
        {
			base.Init(moveTransform, characterEngine, tileChecker);
            _moveTransform = moveTransform;
			_battleGround = moveTransform.GetComponentInParent<IBattleGround>();
            _battleManager = moveTransform.GetComponentInParent<IBattleManager>();
            _characterEngine.SubscribeOnMoveFinish(OnMoveFinish);
		}

        private void OnMoveFinish()
        {
            if (_dead) return;
            _moveBehaviour.RecalcMoveDirection();
            var direction = MoveVector.Direction();
            _tileChecker.UpdateChecker(direction);
            if (_tileChecker.CouldAttack() || _tileChecker.CouldMove()) return;
            _activeCells = _deltaPoses.Where(AvailableCreate).Select(GetCheckPos).ToArray();
            foreach (var activeCell in _activeCells)
            {
                var preview = Instantiate(_minionPreviewPrefab, activeCell, Quaternion.identity, transform);
                _minionPreviews.Add(preview);
            }
            Debug.Log($"_activeCells - {_activeCells.Count()} {Time.time}");
            CellHighlightManager.Instance.SetCellsColor(GetInstanceID(),
                _activeCells,
                CellHighlightType.Yellow);
            Debug.Log($"Highligh {Time.time}");
        }

        public void CreateMinion(Vector2 pos)
        {
            Debug.Log($"create minion {Time.time}");
            var minion = Instantiate(_minionPrefab, pos, Quaternion.identity, _battleGround.BattleTransform);
            var monster = minion.GetComponent<IMonster>();
			var minionBehaviour = minion.GetComponent<IDependencyBehaviour>();
            minionBehaviour.SetControlMonster(_moveTransform.gameObject);
            _battleManager.Register(monster);
		}

		public override void MakeMove()
		{
			if (MovesRemain < 1) return;
			_moveBehaviour.RecalcMoveDirection();
			var direction = MoveVector.Direction();
			_animManager.Turn(direction);
			_tileChecker.UpdateChecker(direction);
            if (_tileChecker.CouldAttack())
            {
                _characterEngine.OnMoveMade(false);
                //_characterEngine.SpendMove(true);
            } else if (_tileChecker.CouldMove())
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

        public override bool Wait()
        {
            var result = base.Wait();
			TryCreateMinion();
            return result;
        }

        private Vector2 GetCheckPos(Vector2 deltaPos)
        {
            return (Vector2)_moveTransform.position + deltaPos;
        }

        private bool AvailableCreate(Vector2 deltaPos)
        {
            var creationPos = (Vector2)_moveTransform.position + deltaPos;
            var tileData = _battleGround.GetTileData(creationPos);
            var monster = _battleGround.CheckComponent<ICharacterCollider>(creationPos);
            var PlayerNextPos = _battleGround.PlayerNextPos;
            return tileData.slotType == SlotType.Empty && monster == null && creationPos != PlayerNextPos;
        }

        private void TryCreateMinion()
        {
            foreach (var minionPreview in _minionPreviews)
            {
                minionPreview.SetActive(false);
            }
            _minionPreviews.Clear();
            CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
            Debug.Log($"create monsters _activeCells - {_activeCells.Count()} {Time.time}");
            foreach (var creationPos in _activeCells)
            {
                var tileData = _battleGround.GetTileData(creationPos);
                var monster = _battleGround.CheckComponent<ICharacterCollider>(creationPos);
                var PlayerNextPos = _battleGround.PlayerNextPos;
                if (tileData.slotType == SlotType.Empty && monster == null && creationPos != PlayerNextPos)
                {
                    CreateMinion(creationPos);
                }
            }
        }

        public void OnDeathEvent()
        {
            _dead = true;
            foreach (var minionPreview in _minionPreviews)
            {
                minionPreview.SetActive(false);
            }
            _minionPreviews.Clear();
            CellHighlightManager.Instance.RemoveCellsColor(GetInstanceID());
        }
    }
}