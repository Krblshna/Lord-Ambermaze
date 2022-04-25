using System.Collections.Generic;
using AZ.Core.Conditions;
using UnityEngine;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public class VectorDistanceCondition : PlayerDistanceCondition
    {
        [SerializeField] private float _distanceToPlayer;

        protected override void SetInitCells()
        {
            int maxCheck = (int)Mathf.Floor(_distanceToPlayer);
            for (int i = -maxCheck; i <= maxCheck; i++)
            {
                for (int j = -maxCheck; j <= maxCheck; j++)
                {
                    var calcDistance = new Vector2(i, j).magnitude;
                    if (calcDistance <= _distanceToPlayer)
                    {
                        _zoneCellsOffset.Add(new Vector2(i, j));
                    }
                }
            }
        }
        protected override bool ConditionCheck()
        {
            _zoneCells.Clear();
            var playerPos = _aImanager.PlayerPos;
            var monsterPos = _moveTransform.position;
            float calcDistance = Vector2.Distance(playerPos, monsterPos);
            return SimpleCondition.Check(calcDistance, _distanceToPlayer, _comparator);
        }
    }
}