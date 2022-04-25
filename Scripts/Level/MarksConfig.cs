using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Level
{
    public enum MarkType
    {
        None,
        Portal,
        Exclamation
    };
    [System.Serializable]
    public class RoomMark
    {
        public int SceneId;
        public MarkType Mark;
    }

    [CreateAssetMenu(fileName = "MarksConfig", menuName = "ScriptableObjects/MarksConfig", order = 1)]
    public class MarksConfig : ScriptableObject
    {
        public RoomMark[] Data;

        public MarkType GetMarkType(RoomData roomData)
        {
            foreach (var roomMark in Data)
            {
                if (roomMark.SceneId == roomData.SceneId) return roomMark.Mark;
            }

            return MarkType.None;
        }
    }
}