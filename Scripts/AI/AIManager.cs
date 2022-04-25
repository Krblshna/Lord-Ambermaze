using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.A_Star;
using AZ.Core;
using LordAmbermaze.Battle;
using LordAmbermaze.Core;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.AI
{
    public class AIManager : MonoBehaviour
    {
        private IPath _path;
        private IPlayer _player;
        private IBattleGround _battleGround;
        private ObstaclesDetector _obstaclesDetector;
        void Awake()
        {
            _path = new Path();
            _player = GetComponentInChildren<IPlayer>();
            _battleGround = GetComponent<IBattleGround>();
            _obstaclesDetector = GetComponent<ObstaclesDetector>();
        }

        public Vector2 PlayerPos => _battleGround.PlayerNextPos == Vector2.zero
            ? _player.CurrentPos
            : _battleGround.PlayerNextPos;

        public Vector2 GetMoveDirectionToPlayer(Transform enemyTransform, IEnumerable<SlotType> obstacleSlots)
        {
            var startPos = enemyTransform.position.ToVector2Int();
            var playerPos = PlayerPos.ToVector2Int();
            var obstacles = _obstaclesDetector.GetObstacles(obstacleSlots);
            if (obstacles.Contains(playerPos))
            {
                obstacles.Remove(playerPos);
            }

            IReadOnlyCollection<Vector2Int> rightPath;
            if (!_path.Calculate(startPos, playerPos, obstacles, out rightPath))
            {
                return Vector2.zero;
            }

            var fullObstacles = _obstaclesDetector.GetMonstersAndObstacles(obstacleSlots);
            if (fullObstacles.Contains(playerPos))
            {
                fullObstacles.Remove(playerPos);
            }
            if (_path.Calculate(startPos, playerPos, fullObstacles, out var pathToTarget2))
            {
                rightPath = pathToTarget2.Count <= rightPath.Count ? pathToTarget2 : rightPath;
            }

            return MoveDirectionByPath(rightPath);
        }

        public Vector2 GetDirectionToCell(Vector2Int startPos, Vector2Int endPos, IEnumerable<SlotType> obstacleSlots)
        {
            var obstacles = _obstaclesDetector.GetObstacles(obstacleSlots);
            IReadOnlyCollection<Vector2Int> rightPath;
            if (!_path.Calculate(startPos, endPos, obstacles, out rightPath))
            {
                return Vector2.zero;
            }
            return MoveDirectionByPath(rightPath);
        }

        private Vector2 MoveDirectionByPath(IReadOnlyCollection<Vector2Int> pathToTarget)
        {
            var currentPos = pathToTarget.ElementAt(pathToTarget.Count - 1);
            var nextMovePos = pathToTarget.ElementAt(pathToTarget.Count - 2);
            return new Vector2(Mathf.RoundToInt(nextMovePos.x - currentPos.x), Mathf.RoundToInt(nextMovePos.y - currentPos.y));
        }
    }
}