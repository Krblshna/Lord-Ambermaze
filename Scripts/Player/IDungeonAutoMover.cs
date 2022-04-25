using System;
using LordAmbermaze.Animations;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public interface IDungeonAutoMover
	{
		void Init(Transform moveTransform, IAnimManager animManager);
        void MoveToLevelDungeon(Vector2Int direction, Action callback);
        void InstantMove(Vector2 pos);
        void MoveFromLevelDungeon(Vector2Int direction, Action action);
    }
}