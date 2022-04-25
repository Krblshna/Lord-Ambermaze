using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Level
{
    [CreateAssetMenu(fileName = "GameBiomsConfig", menuName = "ScriptableObjects/GameBiomsConfig", order = 1)]
    public class GameBiomsConfig : ScriptableObject
    {
        public BiomConfig[] Bioms;
    }
}