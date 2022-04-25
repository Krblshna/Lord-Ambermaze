using System;
using System.Collections;
using System.Text.RegularExpressions;
using AZ.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
//using LordAmbermaze.Camera;
using LordAmbermaze.Core;
using Coroutine = AZ.Core.Coroutine;

namespace LordAmbermaze.ScenesManagement
{
	public class ScenesManager : MonoBehaviour
	{
		//[SerializeField]
		//private CameraManager _cameraManager;
        private static string _gameplayScene = "Gameplay", _mapScene = "Map", _menuScene = "Main menu";
		private static string SceneName(int roomNum) => $"B{_currentBiom}-L{_currentLevel}-Room {roomNum}";
        private static int _currentRoom, _previousRoom, _currentLevel, _currentBiom;
        private bool _needUnloadPrevScene;

		public void Awake()
		{
			SetCurrentRoom();
		}

        void Start()
        {
			EventManager.StartListening("BlenFinished", UnloadPrevScene);
            EventManager.StartListening(EventList.UnloadPrevScene, UnloadPrevScene);
            EventManager.StartListening(EventList.ForceUnloadPrevScene, ForceUnloadPrevScene);
		}

		private void SetCurrentRoom()
		{
			var activeScene = SceneManager.GetActiveScene();
			var sceneName = activeScene.name;
			var roomStr = Regex.Match(sceneName, @"\d+").Value;
			if (int.TryParse(roomStr, out int room))
			{
				_currentRoom = room;
			}
		}

		public void LoadRoom(int room)
        {
            if (_needUnloadPrevScene) return;
            
			UpdateRoomInfo(room);
			LoadRoomScene(_currentRoom);
            _needUnloadPrevScene = true;
			if (_previousRoom != 0)
			{
				
                
                //_cameraManager.OnCameraBlendFinish(UnloadPrevScene);
            }

		}

        public void GoToMainMenu()
        {
            Coroutine.Instance.StartCoroutine(MainMenuProcess());
		}

        public static void FinishGame()
        {
            Coroutine.Instance.StartCoroutine(FinishGameProcess());
        }

        private static IEnumerator FinishGameProcess()
        {
            CommonBlocker.Block(CommonBlocks.Loading);
            EventManager.TriggerEvent(EventList.ScreenHide);
            yield return new WaitForSeconds(1);
            AsyncOperation menuLoading = SceneManager.LoadSceneAsync("Main menu");
            while (!menuLoading.isDone)
            {
                yield return null;
            }

            EventManager.TriggerEvent(EventList.ScreenShow);
            CommonBlocker.Unblock(CommonBlocks.Loading);
        }

        private IEnumerator MainMenuProcess()
        {
            CommonBlocker.Block(CommonBlocks.Loading);
            EventManager.TriggerEvent(EventList.ScreenHide);
            yield return new WaitForSeconds(1);
            UnloadPrevScene();
			AsyncOperation menuLoading = SceneManager.LoadSceneAsync("Main menu");
            while (!menuLoading.isDone)
            {
                yield return null;
            }

            EventManager.TriggerEvent(EventList.ScreenShow);
            CommonBlocker.Unblock(CommonBlocks.Loading);
        }

		public static void UpdateRoomInfo(int room)
        {
            _previousRoom = _currentRoom;
            _currentRoom = room;
		}

        public static void StartInit()
        {
            SceneManager.LoadScene("Init");
        }

        public static void StartComics()
        {
            SceneManager.LoadScene("Start Comics");
        }

        public static void StartNewLevel(int room = 1, int level = -1, int biom = -1)
        {
            if (level >= 0)
            {
                _currentLevel = level;
            }
            if (biom >= 0)
            {
                _currentBiom = biom;
            }
            UpdateRoomInfo(room);
            Coroutine.Instance.StartCoroutine(ChangeLevel(room));
        }

        public static void StartNewGame(int room = 1, int level = -1, int biom = -1)
        {
            if (level >= 0)
            {
                _currentLevel = level;
            }
            if (biom >= 0)
            {
                _currentBiom = biom;
            }
            Coroutine.Instance.StartCoroutine(NewGameProcess(room));
        }

        private static IEnumerator ChangeLevel(int room)
        {
            CommonBlocker.Block(CommonBlocks.Loading);
            AsyncOperation mapUnloading = SceneManager.UnloadSceneAsync(_mapScene);
            while (!mapUnloading.isDone)
            {
                yield return null;
            }

            AsyncOperation mapLoading = SceneManager.LoadSceneAsync(_mapScene, LoadSceneMode.Additive);
            while (!mapLoading.isDone)
            {
                yield return null;
            }
            AsyncOperation roomLoading = LoadRoomS(room);
            while (!roomLoading.isDone)
            {
                yield return null;
            }
            EventManager.TriggerEvent(EventList.ScreenShow);
            CommonBlocker.Unblock(CommonBlocks.Loading);
        }

        public static void RestartRoom(Func callback)
        {
            Coroutine.Instance.StartCoroutine(RestartRoomProcess(callback));
        }

        private static IEnumerator RestartRoomProcess(Func callback)
        {
            AsyncOperation gameplayLoading = SceneManager.UnloadSceneAsync(SceneName(_currentRoom));
			while (!gameplayLoading.isDone)
            {
                yield return null;
            }
            AsyncOperation roomLoading = LoadRoomS(_currentRoom);
            while (!roomLoading.isDone)
            {
                yield return null;
            }
            callback?.Invoke();
        }

		private static IEnumerator NewGameProcess(int room)
        {
            CommonBlocker.Block(CommonBlocks.Loading);
            EventManager.TriggerEvent(EventList.ScreenHide);
            yield return new WaitForSeconds(1);
            AsyncOperation gameplayLoading = SceneManager.LoadSceneAsync(_gameplayScene, LoadSceneMode.Additive);
			while (!gameplayLoading.isDone)
            {
                yield return null;
            }

            AsyncOperation mapLoading = SceneManager.LoadSceneAsync(_mapScene, LoadSceneMode.Additive);
            while (!mapLoading.isDone)
            {
                yield return null;
            }

            var scenesManager = FindObjectOfType<ScenesManager>();
			UpdateRoomInfo(room);
			AsyncOperation roomLoading = LoadRoomS(room);
            while (!roomLoading.isDone)
            {
                yield return null;
            }
            //SceneManager.UnloadSceneAsync(_menuScene);
			EventManager.TriggerEvent(EventList.ScreenShow);
            CommonBlocker.Unblock(CommonBlocks.Loading);
		}

        private void ForceUnloadPrevScene()
        {
            var sceneName = SceneName(_currentRoom);
            StartCoroutine(UnloadSceneProcess(sceneName));
        }

		private void UnloadPrevScene()
        {
            if (!_needUnloadPrevScene) return;
			_needUnloadPrevScene = false;
			var sceneName = SceneName(_previousRoom);
			StartCoroutine(UnloadSceneProcess(sceneName));
		}

		private void LoadRoomScene(int room)
		{
			var sceneName = SceneName(room);
			StartCoroutine(LoadSceneProcess(sceneName));
		}

		public void LoadScene(string sceneName)
		{
			StartCoroutine(LoadSceneProcess(sceneName));
		}

		public static AsyncOperation LoadSceneS(string sceneName)
		{
			return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}

        public static AsyncOperation LoadRoomS(int room)
        {
            var sceneName = SceneName(room);
            return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public static AsyncOperation UnloadSceneS(string sceneName)
        {
            return SceneManager.UnloadSceneAsync(sceneName);
		}

		private IEnumerator UnloadSceneProcess(string sceneName)
		{
			AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName);
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
			//Debug.Log($"Scene Unloaded - {sceneName}");
		}

		private IEnumerator LoadSceneProcess(string sceneName)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
            EventManager.TriggerEvent("CheckBlend");
            EventManager.TriggerEvent(EventList.SceneLoaded);
			//Debug.Log($"Scene Loaded - {sceneName}");
		}

		private bool WasSceneLoaded(string sceneName)
		{
			if (SceneManager.sceneCount > 0)
			{
				for (int i = 0; i < SceneManager.sceneCount; ++i)
				{
					Scene scene = SceneManager.GetSceneAt(i);
					if (scene.name == sceneName)
					{
						return true;
					}
				}
			}

			return false;
		}

        public static void FinishDemo()
        {
            Coroutine.Instance.StartCoroutine(DemoLoadingProcess());
        }

        private static IEnumerator DemoLoadingProcess()
        {
            CommonBlocker.Block(CommonBlocks.Loading);
            AsyncOperation menuLoading = SceneManager.LoadSceneAsync("Demo Finish");
            while (!menuLoading.isDone)
            {
                yield return null;
            }

            EventManager.TriggerEvent(EventList.ScreenShow);
            CommonBlocker.Unblock(CommonBlocks.Loading);
        }
    }
}