using System.Collections.Generic;
using LordAmbermaze.Level;
using UnityEngine;

namespace LordAmbermaze.UI.Map
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] private GameObject roomPrefab;
        [SerializeField] private Transform roomContainer;
        private LevelConfig _levelConfig;
        private MarksConfig _marksConfig;
        private bool _initialized;

        public void Awake()
        {
            Init();
            if (_levelConfig == null) return;
            foreach (var roomData in _levelConfig.Data)
            {
                var roomObj = Instantiate(roomPrefab, roomContainer);
                var room = roomObj.GetComponent<Room>();
                var markType = _marksConfig.GetMarkType(roomData);
                room.Init(roomData, markType, _levelConfig.StartPos);
            }

            GetComponent<MapManager>().Init();
        }

        private void Init()
        {
            if (_initialized) return;
            var levelManager = LevelManager.Instance;
            if (levelManager == null) return;
            _initialized = true;
            _levelConfig = levelManager.LevelConfig;
            _marksConfig = levelManager.MarksConfig;
        }
    }
}