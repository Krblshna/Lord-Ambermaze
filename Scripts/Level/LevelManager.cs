using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using AZ.Core.Depot;
using LordAmbermaze.Music;
using LordAmbermaze.Player;
using LordAmbermaze.ScenesManagement;
using UnityEngine;


namespace LordAmbermaze.Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameBiomsConfig _gameBiomsConfig;
        private LevelConfig _levelConfig;
        private MarksConfig _marksConfig;
        private BiomConfig _biomConfig;

        public LevelConfig LevelConfig
        {
            get
            {
                if (!_init)
                {
                    Init();
                }

                return _levelConfig;
            }
        }

        public MarksConfig MarksConfig
        {
            get
            {
                if (!_init)
                {
                    Init();
                }

                return _marksConfig;
            }
        }
        public Vector2Int Pos => _pos;
        public int CurLevel { get; private set; }
        public int CurBiom { get; private set; }

        public string RoomReward => $"biom-{CurBiom}level-{CurLevel}-pos-{_pos}";

        private static LevelManager _mInstance;
        private Dictionary<int, BiomConfig> _biomsDict;
        private Dictionary<int, LevelConfig> _levelsDict;
        private Dictionary<Vector2Int, RoomData> _roomsDict;
        private Vector2Int _pos;
        private bool _init;
        public static LevelManager Instance
        {
            get
            {
                if (_mInstance == null)
                {
                    _mInstance = (LevelManager)FindObjectOfType(typeof(LevelManager));
                }

                return _mInstance;
            }
        }
        private void Awake()
        {
            if (_mInstance != null)
            {
                Destroy(this);
            }
            else
            {
                //GameMaster.Load();
                _mInstance = this;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            EventManager.StartListening(CommonEventList.BeforeSave, BeforeSave);
        }

        public void StartNew()
        {
            //GameMaster.Load();
            Init();
            LevelMusicStarter.Instance.TryStartMusic(CurBiom, CurLevel);
            _pos = Storage.Instance.data.roomPos;
            LoadInitRoom();
        }

        public void LoadLevelConfig(Vector2Int pos, int nextLevelId, int nextBiomId)
        {
            if (nextBiomId >= 0)
            {
                CurBiom = nextBiomId;
                _biomConfig = _biomsDict[CurBiom];
                _levelsDict = _biomConfig.Levels.ToDictionary(level => level.LevelId, level => level);
            }

            _pos = pos;
            CurLevel = nextLevelId;
            _levelConfig = _levelsDict[CurLevel];
            _marksConfig = _levelConfig.MarksConfig;
            _roomsDict = _levelConfig.Data.ToDictionary(data => data.Pos, data => data);

            StartNewLevel();
            new Reward(StorageType.Counters, RoomReward, 1).Claim();
            Storage.Instance.Save("save0");
        }

        private void BeforeSave()
        {
            Storage.Instance.SaveRoomPos(_pos, CurLevel, CurBiom);
        }

        private void Init()
        {
            _init = true;
            CurBiom = Storage.Instance.data.CurBiom;
            CurLevel = Storage.Instance.data.CurLevel;
            _biomsDict = _gameBiomsConfig.Bioms.ToDictionary(biom => biom.BiomId, biom => biom);
            _biomConfig = _biomsDict[CurBiom];
            _levelsDict = _biomConfig.Levels.ToDictionary(level => level.LevelId, level => level);
            _levelConfig = _levelsDict[CurLevel];
            _marksConfig = _levelConfig.MarksConfig;
            _roomsDict = _levelConfig.Data.ToDictionary(data => data.Pos, data => data);
        }

        private void StartNewLevel()
        {
            var roomData = _roomsDict[_pos];
            new Reward(StorageType.Counters, RoomReward, 1).Claim();
            ScenesManager.StartNewLevel(roomData.SceneId, CurLevel, CurBiom);
        }

        private void LoadInitRoom()
        {
            var roomData = _roomsDict[_pos];
            new Reward(StorageType.Counters, RoomReward, 1).Claim();
            ScenesManager.StartNewGame(roomData.SceneId, CurLevel, CurBiom);
        }

        public void GateTransition(Vector2Int delta)
        {
            _pos += delta;
            var roomData = _roomsDict[_pos];
            var scenesManager = FindObjectOfType<ScenesManager>();
            new Reward(StorageType.Counters, RoomReward, 1).Claim();
            Storage.Instance.Save("save0");
            scenesManager.LoadRoom(roomData.SceneId);
        }

        public void InstantTeleport(Vector2Int newPos)
        {
            _pos = newPos;
            var roomData = _roomsDict[newPos];
            var scenesManager = FindObjectOfType<ScenesManager>();
            Storage.Instance.Save("save0");
            scenesManager.LoadRoom(roomData.SceneId);
            EventManager.TriggerEvent("UnloadPrevScene");
        }

        public IEnumerable<RoomLink> GetCurRoomLinks()
        {
            return _roomsDict[_pos].RoomsLinks;
        }


        public void FinishDemo()
        {
            ScenesManager.FinishDemo();
        }
    }
}