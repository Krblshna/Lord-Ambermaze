using System.Collections.Generic;
using AZ.Core.Conditions;
using UnityEngine;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public class CellDistanceCondition : PlayerDistanceCondition
    {
        [SerializeField] protected int _distanceToPlayer;

        protected override void SetInitCells()
        {
            for (int i = -_distanceToPlayer; i <= _distanceToPlayer; i++)
            {
                for (int j = -_distanceToPlayer; j <= _distanceToPlayer; j++)
                {
                    _zoneCellsOffset.Add(new Vector2(i, j));
                }
            }
        }

        protected override bool ConditionCheck()
        {
            var playerPos = _aImanager.PlayerPos;
            var monsterPos = _moveTransform.position;
            int distanceX = Mathf.RoundToInt(Mathf.Abs(playerPos.x - monsterPos.x));
            int distanceY = Mathf.RoundToInt(Mathf.Abs(playerPos.y - monsterPos.y));
            int calcDistance = Mathf.Max(distanceX, distanceY);
            return SimpleCondition.Check(calcDistance, _distanceToPlayer, _comparator);
        }
    }
}