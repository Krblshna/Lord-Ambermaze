using UnityEngine;

namespace LordAmbermaze.Level
{
    public class LevelStarter : MonoBehaviour
    {
        void Start()
        {
            LevelManager.Instance.StartNew();
        }
    }
}