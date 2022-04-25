using System;
using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Animations;
using LordAmbermaze.Core;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public class DungeonAutoMover : MonoBehaviour, IDungeonAutoMover
	{
		private Transform _moveTransform;
		private IMover _mover;
		private int moveTimes = 6;
        private int moveSign;
		private Vector2Int _direction;
		private Vector2 _stairsHeight = new Vector2(0, 0.4f);
		private Action _callback;
		private IAnimManager _animManager;

		public void Init(Transform moveTransform, IAnimManager animManager)
		{
			_moveTransform = moveTransform;
			_mover = GetComponent<IMover>();
			_animManager = animManager;
		}

        public void MoveToLevelDungeon(Vector2Int direction, Action callback)
        {
			_callback = callback;
            ChangeSprites(_moveTransform.gameObject, SpriteMaskInteraction.VisibleInsideMask);
            _callback += () =>
            {
                _moveTransform.gameObject.SetActive(false);
            };
            _direction = direction;
            _animManager.Turn(_direction);
            GameMaster.SetTransitionData(_moveTransform.localPosition, _direction);
            moveSign = -1;
			Repeat();
		}

        public void InstantMove(Vector2 pos)
        {
			_moveTransform.localPosition = pos;
		}

        public void MoveFromLevelDungeon(Vector2Int direction, Action callback)
        {
			_callback = callback;
            _callback += () =>
            {
				ChangeSprites(_moveTransform.gameObject, SpriteMaskInteraction.None);
			};
			ChangeSprites(_moveTransform.gameObject, SpriteMaskInteraction.VisibleInsideMask);
            _direction = direction;
			_animManager.Turn(_direction);
            moveSign = 1;
            Repeat();
		}

        private void ChangeSprites(GameObject gameObj, SpriteMaskInteraction visibility)
        {
            var allSprites = gameObj.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in allSprites)
            {
                sprite.maskInteraction = visibility;
            }
        }

		private void Repeat()
		{
			if (moveTimes <= 0)
			{
                _callback?.Invoke();
				return;
			}

			moveTimes--;
			var newPos = (Vector2)_moveTransform.position + _direction + moveSign * _stairsHeight;
			Utils.SetTimeOut(() =>
			{
				_animManager.Play(AnimTypes.move);
				_mover.MoveTo(newPos, Repeat);
			}, 0.1f);
		}
	}
}