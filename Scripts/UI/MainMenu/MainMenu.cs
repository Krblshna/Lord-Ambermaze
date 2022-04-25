using System.Collections;
using AZ.Core;
using AZ.UI;
using AZ.Core.Depot;
using LordAmbermaze.Core;
using LordAmbermaze.Localization;
using LordAmbermaze.Player;
using LordAmbermaze.ScenesManagement;
using UnityEngine;

namespace LordAmbermaze.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        private IMenuController _menuController;
        private bool _used;

        private void Start()
        {
            _menuController = GetComponent<IMenuController>();
            _menuController.Init();
            CommonBlocker.Block(CommonBlocks.MainMenu);
        }

        private void Update()
        {
            if (!CommonBlocker.IsBlocked(CommonBlocks.Menu))
            {
                _menuController.CustomUpdate();
            }
        }

        public void TryStartGame()
        {
            //StartNewGame();
            //return;
            if (Storage.HaveAutosave())
            {
                Confirmer.Instance.ShowConfirmer(Texts.TryStartGame,
                    StartNewGame);
            }
            else
            {
                StartNewGame();
            }
        }

        public void OpenUrl()
        {
            Application.OpenURL("https://store.steampowered.com/app/1811330/Lord_Ambermaze/");
        }

        private void StartNewGame()
        {
            if (_used) return;
            _used = true;
            Storage.Instance.StartNew();
            ScenesManager.StartComics();
            PlayerState.OnRestart();
            GameMaster.StartNew();
            CommonBlocker.Unblock(CommonBlocks.MainMenu);
        }

        public void ContinueGame()
        {
            if (_used) return;
            _used = true;
            Storage.Instance.Load();
            ScenesManager.StartInit();
            PlayerState.OnRestart();
            GameMaster.Load();
            CommonBlocker.Unblock(CommonBlocks.MainMenu);
        }

        public void Exit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
					Application.Quit();
            #endif
        }
    }
}