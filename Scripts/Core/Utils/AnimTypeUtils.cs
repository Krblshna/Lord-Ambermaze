using System.Collections.Generic;
using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
    public static class AnimTypeUtils
    {
        private static readonly Dictionary<Vector2Int, AnimTypes> AnimMoveDict = new Dictionary<Vector2Int, AnimTypes>()
        {
            {Vector2Int.left, AnimTypes.move_left},
            {Vector2Int.right, AnimTypes.move_right},
            {Vector2Int.down, AnimTypes.move_down},
            {Vector2Int.up, AnimTypes.move_up},
        };

        private static readonly Dictionary<Vector2Int, AnimTypes> AnimAttackDict = new Dictionary<Vector2Int, AnimTypes>()
        {
            {Vector2Int.left, AnimTypes.attack_left},
            {Vector2Int.right, AnimTypes.attack_right},
            {Vector2Int.down, AnimTypes.attack_down},
            {Vector2Int.up, AnimTypes.attack_up},
        };
        public static AnimTypes GetMoveAnimType(Vector2 direction)
        {
            var vector = new Vector2Int((int)direction.x, (int)direction.y);
            return AnimMoveDict.ContainsKey(vector) ? AnimMoveDict[vector] : AnimTypes.move;
        }

        public static AnimTypes GetAttackAnimType(Vector2 direction)
        {
            var vector = new Vector2Int((int)direction.x, (int)direction.y);
            return AnimAttackDict.ContainsKey(vector) ? AnimAttackDict[vector] : AnimTypes.attack;
        }
    }
}