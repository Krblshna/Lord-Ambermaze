using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.TurnableObjects.Spikes
{
	enum Stage {Wait, Prepare, Attack}
	public class Spikes : MonoBehaviour, ITurnableObject
	{
		[SerializeField]
		private GameObject _damager;
		[SerializeField] private int _maxWaitTurns = 1;
		[SerializeField] private int phase;
		private IBattleManager _battleManager;
		private IBlocker _blocker;
		private ISpikesGraphics _spikesGraphics;
		private int _waitTurns;
        private Stage _currentStage;

		public void Init(IBattleManager battleManager)
		{
			_blocker = battleManager.Blocker;
			_damager.SetActive(false);
			_battleManager = battleManager;
			_waitTurns = 0;
			_spikesGraphics = GetComponentInChildren<ISpikesGraphics>();
			EventManager.StartListening(EventList.TurnFinished, MakeAttack);
			SetPhase();
		}

		void SetPhase()
		{
			_spikesGraphics.Init();
			_waitTurns -= phase;
			if (phase == 1)
			{
				Prepare();
			} else if (phase == 2)
            {
                _waitTurns = _maxWaitTurns;
				MakeAttack();
				_spikesGraphics.Attack();
			}
		}
		public void MakeMove()
		{
			if (_waitTurns > 0)
			{
				Wait();
			} else if (_currentStage == Stage.Prepare)
			{
				 Attack();
			}
			else
			{
				Prepare();
			}
		}
		private void Attack()
		{
			//_blocker.StartListeningUnblock(Blocks.Movement, MakeAttack);
			_spikesGraphics.Attack();
			OnAttack();
		}
		private void MakeAttack()
        {
            if (_currentStage != Stage.Attack) return;
            //var position = transform.position;
            //position = new Vector2(position.x + 0.000001f, position.y);
            //transform.position = position;
            //_damager.SetActive(true);
            DoDamage();
        }

        private void DoDamage()
        {
			var opponentCollider = _battleManager.BattleGround.GetTileCharCollider(transform.position);
            opponentCollider?.Hit(2, Group.Neutral);
		}

		private void OnAttack()
		{
			SoundManager.PlaySound(SoundType.spikes_out);
            _currentStage = Stage.Attack;
			_waitTurns = _maxWaitTurns;
		}

		private void Prepare()
		{
            _currentStage = Stage.Prepare;
			_spikesGraphics.Prepare();
		}

		private void Wait()
        {
            _currentStage = Stage.Wait;
			_damager.SetActive(false);
			_waitTurns -= 1;
			_spikesGraphics.Wait();
		}
	}
}