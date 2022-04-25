using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.InteractableSlots;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface ICharacterEngine
	{
        Vector2 MoveVector { get; }
		void Init(IBattleManager battleManager, Transform moveTransform, AInteractableSlots interactableSlots);
		void MakeMove();
        void Wait();
		void Move(Func onMoveFinishCallback = null);
		void PrepareAttack(Func onAttackCallback = null);
		void TryConnect();
		void MetObstacle();
		void RefreshMoveSteps();
		void AllMovementsStarted();
		List<TileData> GetUpdatedTilesData();
		void SpendMove(bool all = false);
		void UrgentAttack();
        void Attack(Func onAttackCallback = null);
		void CustomDestroy();
        void Death();
        void Push(Vector2Int pushDirection);
        void SubscribeOnMoveFinish(Func onMove);
        void SubscribeOnMoveStart(Func onMove);
        void OnMoveMade(bool moved);

    }
}