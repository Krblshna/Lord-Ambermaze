using System.Linq;
using LordAmbermaze.Level;
using UnityEngine;

namespace LordAmbermaze.Environment
{
    [System.Serializable]
    public class Pass
    {
        public GameObject Gate;
        public GameObject GateBlock;
        public RoomLink Link;
    }

    public class PassManager : MonoBehaviour
    {
        [SerializeField]
        private Pass[] _passes;
        void Start()
        {
            var levelManager = LevelManager.Instance;
            if (levelManager == null) return;
            var roomLinks = levelManager.GetCurRoomLinks();
            foreach (var pass in _passes)
            {
                var active = roomLinks.Contains(pass.Link);
                pass.Gate.SetActive(active);
                pass.GateBlock.SetActive(!active);
            }
        }
    }
}