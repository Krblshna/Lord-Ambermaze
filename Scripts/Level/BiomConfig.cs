using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Level
{
    [CreateAssetMenu(fileName = "BiomConfig", menuName = "ScriptableObjects/BiomConfig", order = 1)]
    public class BiomConfig : ScriptableObject
    {
        public int BiomId;
        public LevelConfig[] Levels;
    }
}