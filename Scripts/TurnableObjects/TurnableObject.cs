using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.InteractableSlots;
using UnityEngine;

namespace LordAmbermaze.TurnableObjects
{
    public class TurnableObject : MonoBehaviour, ITurnableObject, IDamageable, IHaveDamageEvent
    {
        private ICharacterEngine _characterEngine;
        private ICharacterCollider _collider;
        private IBattleManager _battleManager;

        public Func OnHit { get; set; }

        protected virtual AInteractableSlots InteractableSlots { get; } = new MonsterInteractableSlots();
        public void Init(IBattleManager battleManager)
        {
            _collider = GetComponentInChildren<ICharacterCollider>();
            _battleManager = battleManager;
            _characterEngine = GetComponentInChildren<ICharacterEngine>();

            _collider.Init(this, null, Group.Neutral);
            _characterEngine.Init(battleManager, transform, InteractableSlots);
        }

        public void MakeMove()
        {
            _characterEngine.MakeMove();
        }

        public void GetHit(int damage)
        {
            OnHit?.Invoke();
        }
    }
}