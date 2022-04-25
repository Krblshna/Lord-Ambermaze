using System;
using AZ.Core;
using LordAmbermaze.Battle;
using UnityEngine;
using LordAmbermaze.Player;
using LordAmbermaze.Teleport;

namespace LordAmbermaze.ScenesManagement
{
	public class SceneTransition : MonoBehaviour
	{
		private IBlocker _blocker;
        private IBattleManager _battleManager;
		private Action _callback;
		private IAutoMover _autoMover;
        private IDungeonAutoMover _dungeonAutoMover;

		void Awake()
		{
			_blocker = GetComponent<IBlocker>();
            _battleManager = GetComponent<IBattleManager>();
			_autoMover = GetComponentInChildren<IAutoMover>();
            _dungeonAutoMover = GetComponentInChildren<IDungeonAutoMover>();
        }

        private void BeforeSave()
        {
            GameMaster.Save();
        }

		void Start()
        {
			//GameMaster.Load();
            EventManager.StartListening(CommonEventList.BeforeSave, BeforeSave);
			if (GameMaster.LastTransitionType == SceneTransitionType.Teleport)
            {
                var teleportCell = GetComponentInChildren<ITeleportCell>();
                if (teleportCell == null) return;
                CommonBlocker.Block(CommonBlocks.Battle);
				_autoMover.InstantMove(teleportCell.GetPos);
                SoundManager.PlaySound(SoundType.teleport_end);
                TeleportConnector.Instance.PlayTeleportArrival(() =>
                {
                    CommonBlocker.Unblock(CommonBlocks.Battle);
                });
				return;
            }
            if (GameMaster.LastTransitionType == SceneTransitionType.UndergroundPass)
            {
                var undergroundCell = GetComponentInChildren<IUndergroundCell>();
                if (undergroundCell == null) return;
                CommonBlocker.Block(CommonBlocks.Battle);
                _dungeonAutoMover.InstantMove(undergroundCell.GetPos);
                var exitDirection = undergroundCell.ExitDirection;
                _dungeonAutoMover.MoveFromLevelDungeon(exitDirection, () =>
                {
                    CommonBlocker.Unblock(CommonBlocks.Battle);
                });
                return;
            }

            if (GameMaster.LastLocalPosition == Vector2.zero)
            {
                return;
            }

            
			CommonBlocker.Block(CommonBlocks.Battle);
            GameMaster.GatesOpened = true;
			_autoMover.MoveFromLocationBorder(() =>
			{
				CommonBlocker.Unblock(CommonBlocks.Battle);
                _battleManager.CheckLevelComplete();
            });
		}

		public void WalkToLocationEnd(Action callback = null)
		{
			_callback = callback;
            StartWalkToEnd();
        }

        public void WalkToLevelDungeon(Vector2Int direction, Action callback = null)
        {
            _callback = callback;
            StartWalkToLevelDungeon(direction);
        }

		private void StartWalkToEnd()
		{
			_blocker.Block(Blocks.Movement);
			_autoMover.MoveToLocationBorder(_callback);
		}

        private void StartWalkToLevelDungeon(Vector2Int direction)
        {
            _dungeonAutoMover.MoveToLevelDungeon(direction, _callback);
        }
	}
}