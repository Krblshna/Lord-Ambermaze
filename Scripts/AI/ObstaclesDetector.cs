using System;
using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LordAmbermaze.AI
{
    public class ObstaclesDetector : MonoBehaviour
    {
        private ITileDetector _tileDetector;
        private IBattleGround _battleGround;
        
        private Vector2 _minCorner = new Vector2(-7.5f, -4.5f), _maxCorner = new Vector2(7.5f, 3.5f);
        private readonly Vector2[] gatesPos = new Vector2[]
        {
            new Vector2(-7.5f, 0.5f), new Vector2(-7.5f, -0.5f),
            new Vector2(7.5f, 0.5f), new Vector2(7.5f, -0.5f),
            new Vector2(-0.5f, 3.5f), new Vector2(0.5f, 3.5f),
            new Vector2(-0.5f, -4.5f), new Vector2(0.5f, -4.5f),
        };
        private IEnumerable<Vector2Int> gatesPosInt;
        private readonly Dictionary<SlotType, List<Vector2Int>> _obstaclesPos =
            new Dictionary<SlotType, List<Vector2Int>>();

        private Vector2 initPosDelta;

        public void Awake()
        {
            _tileDetector = GetComponent<ITileDetector>();
            _battleGround = GetComponent<IBattleGround>();
        }

        private void Start()
        {
            initPosDelta = transform.position;
            Init();
            gatesPosInt = gatesPos.Select(vector => (vector + initPosDelta).ToVector2Int());
        }

        private void Init()
        {
            var sizeX = Mathf.RoundToInt(_maxCorner.x - _minCorner.x);
            var sizeY = Mathf.RoundToInt(_maxCorner.y - _minCorner.y);
            for (var xDelta = 0; xDelta <= sizeX; xDelta++)
            {
                for (var yDelta = 0; yDelta <= sizeY; yDelta++)
                {
                    var pos = _minCorner + new Vector2(xDelta, yDelta);
                    var slotType = _tileDetector.GetTileSlotType(pos);
                    AddObstacle(slotType, pos + initPosDelta);
                }
            }
        }

        private void AddObstacle(SlotType slotType, Vector2 pos)
        {
            if (!_obstaclesPos.TryGetValue(slotType, out var slotList))
            {
                slotList = new List<Vector2Int>();
                _obstaclesPos.Add(slotType, slotList);
            }
            slotList.Add(pos.ToVector2Int());
        }

        public IEnumerable<Vector2Int> GetMonstersCurrentPos()
        {
            var monstersList = new List<Vector2Int>();
            var nextMovePositions = _battleGround.TilesData;
            return nextMovePositions.Where(nextMoveData => nextMoveData.Value.slotType == SlotType.Enemy)
                .Select(nextMoveData => nextMoveData.Key);
        }

        public IEnumerable<Vector2Int> GetMonstersNextPos()
        {
            var monstersList = new List<Vector2Int>();
            var nextMovePositions = _battleGround.NextMovePositions;
            return nextMovePositions.Where(nextMoveData => nextMoveData.Value == SlotType.Enemy)
                .Select(nextMoveData => nextMoveData.Key);
        }

        public ICollection<Vector2Int> GetMonstersAndObstacles(IEnumerable<SlotType> obstacleSlots)
        {
            var fullList = new List<Vector2Int>();
            var monstersCurrentCoordinates = GetMonstersCurrentPos();
            var monstersNextCoordinates = GetMonstersNextPos();
            var obstaclesCoordinates = GetObstacles(obstacleSlots);
            fullList.AddRange(monstersCurrentCoordinates);
            fullList.AddRange(obstaclesCoordinates);
            fullList.AddRange(monstersNextCoordinates);

            if (!GameMaster.GatesOpened)
            {
                fullList.AddRange(gatesPosInt);
            }

            return fullList;
        }

        public ICollection<Vector2Int> GetObstacles(IEnumerable<SlotType> obstacleSlots)
        {
            var sumObstaclesList = new List<Vector2Int>();
            foreach (var slotType in obstacleSlots)
            {
                if (_obstaclesPos.TryGetValue(slotType, out var slotObstaclesList))
                {
                    sumObstaclesList.AddRange(slotObstaclesList);
                }
            }

            return sumObstaclesList;
        }
    }
}