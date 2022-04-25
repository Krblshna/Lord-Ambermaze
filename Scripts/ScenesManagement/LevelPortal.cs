using System.Collections;
using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Level;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.ScenesManagement
{
	public class LevelPortal : MonoBehaviour
	{
		[SerializeField]
		private Vector2Int _moveDirection;
        [SerializeField] private Vector2Int _pos = new Vector2Int();
        [SerializeField] private int _nextLevelId;
        [SerializeField] private int _nextBiomId = -1;

		private SceneTransition _sceneTransition;
        private bool _used;
        private IBlocker _blocker;

		void Start()
        {
            _sceneTransition = transform.root.GetComponentInChildren<SceneTransition>();
            _blocker = _sceneTransition.gameObject.GetComponent<IBlocker>();
        }

		void OnTriggerEnter2D(Collider2D collider)
		{
			if (!_used && collider.CompareTag(Tags.Player) && !CommonBlocker.IsBlocked(CommonBlocks.Battle))
			{
                WaitForMoveFinished();
			}
		}

        private void WaitForMoveFinished()
        {
            _blocker.StartListeningUnblock(Blocks.Movement, PrepareChangeLocation);
		}

		private void PrepareChangeLocation()
        {
            _used = true;
			CommonBlocker.Block(CommonBlocks.Battle);
            SoundManager.PlaySound(SoundType.level_transition);
			_sceneTransition.WalkToLevelDungeon(_moveDirection, StartTransition);
        }

        public void StartTransition()
        {
            StartCoroutine(TransitionProcess());
        }

        private IEnumerator TransitionProcess()
        {
            CommonBlocker.Unblock(CommonBlocks.Battle);
            CommonBlocker.Block(CommonBlocks.Loading);
            EventManager.TriggerEvent(EventList.ScreenHide);
            GameMaster.LastTransitionType = SceneTransitionType.UndergroundPass;
            yield return new WaitForSeconds(1);
            EventManager.TriggerEvent(EventList.ForceUnloadPrevScene);
            CommonBlocker.Unblock(CommonBlocks.Loading);
            LevelManager.Instance.LoadLevelConfig(_pos, _nextLevelId, _nextBiomId);

            //EventManager.TriggerEvent(EventList.ScreenShow);
            //CommonBlocker.Unblock(CommonBlocks.Loading);
        }
    }
}