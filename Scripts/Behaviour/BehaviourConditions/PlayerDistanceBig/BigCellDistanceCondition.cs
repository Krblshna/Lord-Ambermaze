using System;
using System.Collections.Generic;
using AZ.Core;
using AZ.Core.Conditions;
using LordAmbermaze.AI;
using LordAmbermaze.Battle;
using LordAmbermaze.CellHighlight;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Behaviour.BehaviourConditions
{
    public class BigCellDistanceCondition : CellDistanceCondition
    {
        private IBodyTiles bodyTiles;

        public override void Init(Transform moveTransform)
        {
            base.Init(moveTransform);
            bodyTiles = moveTransform.GetComponentInChildren<IBodyTiles>();
        }

        protected override void UpdateZone()
        {
            _zoneCells.Clear();
            var monsterPos = _moveTransform.position;
            var bodyTilesPoses = bodyTiles.Tiles;
            foreach (var bodyTilePos in bodyTilesPoses)
            {
                foreach (var zoneOffset in _zoneCellsOffset)
                {
                    var checkTile = zoneOffset + (Vector2) monsterPos + bodyTilePos + new Vector2(0.5f, -0.5f);

                    if (!_zoneCells.Contains(checkTile))
                    {
                        _zoneCells.Add(checkTile);
                    }
                }
            }
        }

        protected override bool ConditionCheck()
        {
            var playerPos = _aImanager.PlayerPos;
            var bodyTilesPoses = bodyTiles.Tiles;
            var monsterPos = _moveTransform.position;

            foreach (var bodyTilePos in bodyTilesPoses)
            {
                var monsterTilePos = (Vector2)monsterPos + bodyTilePos + new Vector2(0.5f, -0.5f);
                int distanceX = Mathf.RoundToInt(Mathf.Abs(playerPos.x - monsterTilePos.x));
                int distanceY = Mathf.RoundToInt(Mathf.Abs(playerPos.y - monsterTilePos.y));
                int calcDistance = Mathf.Max(distanceX, distanceY);
                if (SimpleCondition.Check(calcDistance, _distanceToPlayer, _comparator)) return true;
            }

            return false;

            //int distanceX = Mathf.RoundToInt(Mathf.Abs(playerPos.x - monsterPos.x));
            //int distanceY = Mathf.RoundToInt(Mathf.Abs(playerPos.y - monsterPos.y));
            //int calcDistance = Mathf.Max(distanceX, distanceY);
            //return SimpleCondition.Check(calcDistance, _distanceToPlayer, _comparator);
        }
    }
}