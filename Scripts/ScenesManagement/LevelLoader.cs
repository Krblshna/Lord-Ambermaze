using System.Collections;
using AZ.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LordAmbermaze.ScenesManagement
{
	public class LevelLoader : MonoBehaviour
	{
		[SerializeField]
		private ScenesManager _sceneManager;

		void Start()
		{
			_sceneManager.LoadRoom(1);
			Utils.SetTimeOut(() =>
			{
				_sceneManager.LoadRoom(2);
			}, 5);
		}

		public IEnumerator LoadScene(string sceneName)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
			Debug.Log($"Scene Loaded - {sceneName}");
		}
	}
}