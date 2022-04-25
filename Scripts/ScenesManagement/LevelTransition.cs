using System.Collections;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LordAmbermaze.ScenesManagement
{
    public class LevelTransition : MonoBehaviour
    {
        [SerializeField] private Vector2Int _pos = new Vector2Int();
        [SerializeField] private int _nextLevelId;
        [SerializeField] private int _nextBiomId = -1;
        public void OnTriggerEnter2D(Collider2D coll)
        {
            StartTransition();
        }

        public void StartTransition()
        {
            StartCoroutine(TransitionProcess());
        }

        private IEnumerator TransitionProcess()
        {
            CommonBlocker.Block(CommonBlocks.Loading);
            EventManager.TriggerEvent(EventList.ScreenHide);
            yield return new WaitForSeconds(1);
            EventManager.TriggerEvent(EventList.ForceUnloadPrevScene);
            CommonBlocker.Unblock(CommonBlocks.Loading);
            LevelManager.Instance.LoadLevelConfig(_pos, _nextLevelId, _nextBiomId);

            //EventManager.TriggerEvent(EventList.ScreenShow);
            //CommonBlocker.Unblock(CommonBlocks.Loading);
        }
    }
}