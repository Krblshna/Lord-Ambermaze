using System.Collections.Generic;
using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface IMoveConnector
	{
		ICharacterCollider CharacterCollider { get; }
		bool MoveMade { get; set; }
		void OnMoveMade(bool moved);
		void AddConnection(IMoveConnector moveConnector);
		void OnConnectionMove(IMoveConnector parentConnector, bool moved);
		void Init(IMoveConnecteable connectedElement, ICharacterCollider characterCollider);
		bool CheckCycleConnection();
		bool HaveChildConnector(IMoveConnector moveConnector);
		bool CouldCycleMove();
		bool CheckCycleMovePossibility(HashSet<string> moveSet, IMoveConnector initMoveConnector);
		bool RegisterAttempt(HashSet<string> moveSet);
		void ConnectTo(IMoveConnector moveConnector);
		bool HaveConnector(IMoveConnector moveConnector);
		bool HaveCloseCycleConnection();
	}
}