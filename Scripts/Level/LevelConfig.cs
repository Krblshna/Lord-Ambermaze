using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Level
{
    [System.Serializable]
    public class RoomLock
    {
        public RoomLink Direction;
        public string LockStr;
    }
    public enum RoomLink
    {
        Left,
        Up,
        Right,
        Down
    };
    [System.Serializable]
    public class RoomData
    {
        public int SceneId;
        public Vector2Int Pos;
        public RoomLink[] RoomsLinks;
        public RoomLock[] RoomLocks;
    }

    [CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig", order = 1)]
    public class LevelConfig : ScriptableObject
    {
        public Vector2 StartPos = new Vector2(-91, -27);
        public int LevelId;
        public MarksConfig MarksConfig;
        public RoomData[] Data;
    }
}