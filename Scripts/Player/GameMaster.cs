using AZ.Core;
using AZ.Core.Depot;
using UnityEngine;

namespace LordAmbermaze.Player
{
    public enum SceneTransitionType
    {
        Move, Teleport, UndergroundPass, None
    }

	public static class GameMaster
    {
        public static SceneTransitionType LastTransitionType = SceneTransitionType.None;
        public static Vector2 LastLocalPosition { get; private set; }
		public static Vector2 LastTransitionDirection { get; private set; }
		public static Vector2 ScenePos { get; private set; }

        private static bool _initialized;
        private static bool _gatesOpened;
        public static bool GatesOpened
        {
            get => _gatesOpened;
            set
            {
                _gatesOpened = value;
                EventManager.TriggerEvent("GatesChanged");
            }
        }

        public static void SetTransitionData(Vector2 lastPosition, Vector2 lastDirection)
		{
			LastLocalPosition = lastPosition;
			LastTransitionDirection = lastDirection;
			ScenePos += lastDirection;
		}

        public static void SetTeleportTransition(Vector2 newPos)
        {
            ScenePos = newPos;
        }

        public static void Save()
        {
            Storage.Instance.SaveGameMaster(LastLocalPosition, LastTransitionDirection, ScenePos, GatesOpened,
                (int) LastTransitionType);
        }

        public static void StartNew()
        {
            LastTransitionType = SceneTransitionType.None;
            LastLocalPosition = Vector2.zero;
            LastTransitionDirection = Vector2.zero;
            ScenePos = Vector2.zero;
            GatesOpened = false;
        }

        public static void Load()
        {
            //if (_initialized) return;
            //_initialized = true;
            var data = Storage.Instance.data;
            LastTransitionType = (SceneTransitionType)data.lastTransitionType;
            LastLocalPosition = data.lastLocalPosition;
            LastTransitionDirection = data.lastTransitionDirection;
            ScenePos = data.scenePos;
            GatesOpened = data.gatesOpened;
        }
    }
}