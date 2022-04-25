using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Level;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.ScenesManagement
{
    enum TeleportStage
    {
        none,
        wait_teleport_anim,
        wait_screen_hide,
        wait_new_level_loading
    };
    public class TeleportationTransition : MonoBehaviour
    {
        private bool _teleportationProcess;
        private Vector2Int _teleportPos;
        private TeleportStage _teleportStage;
        private void Start()
        {
            EventManager.StartListening(EventList.OnScreenShowed, OnScreenShowed);
            EventManager.StartListening(EventList.OnScreenHided, OnScreenHided);
            EventManager.StartListening(EventList.SceneLoaded, ScreenShow);
        }
        public void Teleport(Vector2Int teleportPos)
        {
            //block movement
            //Teleport animation and char disappear
            //fade camera screen
            //change location immediately
            //show camera
            //show teleport animation
            //unblock movement
            _teleportPos = teleportPos;
            _teleportationProcess = true;
            GameMaster.LastTransitionType = SceneTransitionType.Teleport;
            GameMaster.SetTeleportTransition(teleportPos);
            CommonBlocker.Block(CommonBlocks.Battle);
            EventManager.TriggerEvent(EventList.TeleportStart);
            _teleportStage = TeleportStage.wait_teleport_anim;
            SoundManager.PlaySound(SoundType.teleport_start);
            TeleportConnector.Instance.PlayTeleportDepature(ScreenHide);
            
        }

        private void ScreenHide()
        {
            _teleportStage = TeleportStage.wait_screen_hide;
            EventManager.TriggerEvent(EventList.ScreenHide);
        }

        private void ScreenShow()
        {
            if (!_teleportationProcess) return;
            EventManager.TriggerEvent(EventList.ScreenShow);
        }

        private void OnScreenHided()
        {
            if (!_teleportationProcess) return;
            if (_teleportStage != TeleportStage.wait_screen_hide) return;
            _teleportStage = TeleportStage.wait_new_level_loading;
            LevelManager.Instance.InstantTeleport(_teleportPos);
        }

        private void OnScreenShowed()
        {
            if (!_teleportationProcess) return;
            if (_teleportStage != TeleportStage.wait_new_level_loading) return;
            _teleportStage = TeleportStage.none;
            CommonBlocker.Unblock(CommonBlocks.Battle);
            EventManager.TriggerEvent(EventList.TeleportFinish);
            _teleportationProcess = false;
        }

        
    }
}