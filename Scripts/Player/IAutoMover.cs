using System;
using LordAmbermaze.Animations;
using UnityEngine;

namespace LordAmbermaze.Player
{
	public interface IAutoMover
	{
		void MoveToLocationBorder(Action callback);
		void Init(Transform moveTransform, IAnimManager animManager);
		void MoveFromLocationBorder(Action callback);
        void InstantMove(Vector2 teleportCellGetPos);
    }
}