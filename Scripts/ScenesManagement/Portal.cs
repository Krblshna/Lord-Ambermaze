using AZ.Core;
using LordAmbermaze.Core;
using LordAmbermaze.Level;
using LordAmbermaze.Player;
using UnityEngine;

namespace LordAmbermaze.ScenesManagement
{
	public class Portal : MonoBehaviour
	{
		[SerializeField]
		private Vector2Int delta;

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
			_sceneTransition.WalkToLocationEnd(ChangeLocation);
        }

		private void ChangeLocation()
		{
			PlayerState.RestoreMana();
			var scenesManager = FindObjectOfType<ScenesManager>();
			CommonBlocker.Unblock(CommonBlocks.Battle);
            GameMaster.LastTransitionType = SceneTransitionType.Move;
            LevelManager.Instance.GateTransition(delta);
			//scenesManager.LoadLevel(NextLevel);
		}
	}
}