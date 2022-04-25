using UnityEngine;
using UnityEngine.SceneManagement;

namespace LordAmbermaze.ScenesManagement
{
	public class GameplayLoader : MonoBehaviour
	{
		private string _sceneName = "Gameplay";
		private bool _isLoaded;
		void Start()
		{
			if (SceneManager.sceneCount > 0)
			{
				for (var i = 0; i < SceneManager.sceneCount; ++i)
				{
					Scene scene = SceneManager.GetSceneAt(i);
					if (scene.name == _sceneName)
					{
						_isLoaded = true;
					}
				}
			}

			if (!_isLoaded)
			{
				ScenesManager.LoadSceneS(_sceneName);
			}
		}
	}
}