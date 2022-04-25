using System;
using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Consumables;
using LordAmbermaze.Core;
using LordAmbermaze.Misc;
using UnityEngine;

namespace LordAmbermaze.DropRewards
{
    [System.Serializable]
    class DropElement
    {
        public GameObject _prefab;
        public DropType _dropType;
    }
    public class DropsManager : Singleton<DropsManager>
    {
        [SerializeField] private DropElement[] _drops;
        private readonly List<EMoveDirection> _availableDirections = new List<EMoveDirection>();
        private IBattleGround _battleGround;
        private IBattleGround BattleGround
        {
            get
            {
                if (_battleGround == null)
                {
                    _battleGround = FindObjectOfType<BattleGround>();
                    _battleGround.SubscribeOnDisable(() => { _battleGround = null;});
                }

                return _battleGround;
            }
        }

        public void CreateDrop(DropType dropType, Vector2 position, int amount = 1, bool clearDirections = true)
        {
            if (clearDirections) ClearDirections();
            while (amount > 0)
            {
                amount -= 1;
                var dropElement = _drops.FirstOrDefault(element => element._dropType == dropType);
                if (dropElement == null) return;
                var obj = Instantiate(dropElement._prefab, position, Quaternion.identity, BattleGround.BattleTransform);
                var consumableAppear = obj.GetComponent<IConsumableAppear>();
                var direction = GetRandDirection(position);
                consumableAppear.Appear(direction);
            }
        }

        private Vector2 GetRandDirection(Vector2 position)
        {
            if (_availableDirections.Count == 0)
            {
                return Vector2.left;
            }
            var randDirection = Utils.RandPop(_availableDirections);
            var direction = Utils.DirectionToVector(randDirection);
            //var direction = Vector2.down;
            var dropPosition = position + direction;
            var tileData = BattleGround.GetGroundTileData(dropPosition);
            var noDropComponent = BattleGround.CheckComponent<NoDrop>(dropPosition);
            if (tileData.slotType != SlotType.Empty || noDropComponent != null)
            {
                return GetRandDirection(position);
            }
            return direction;
        }

        public void CreateDrops(IEnumerable<SimpleDrop> drops, Vector2 position)
        {
            ClearDirections();
            foreach (var simpleDrop in drops)
            {
                if (!Utils.SimpleChance(simpleDrop._dropChance)) continue;
                CreateDrop(simpleDrop._dropType, position, simpleDrop._amount, false);
            }
        }

        private void ClearDirections()
        {
            _availableDirections.Clear();
            var v = Enum.GetValues(typeof(EMoveDirection));
            foreach (var el in v)
            {
                var moveDirection = (EMoveDirection)el;
                if (moveDirection == EMoveDirection.None) continue;
                _availableDirections.Add(moveDirection);
            }
        }
    }
}