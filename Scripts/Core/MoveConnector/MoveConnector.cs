using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public class MoveConnector : MonoBehaviour, IMoveConnector
	{
		private IMoveConnecteable _connectedElement;
		private ICharacterCollider _characterCollider;

		public bool MoveMade { get; set; }
		public ICharacterCollider CharacterCollider => _characterCollider;

		private readonly HashSet<IMoveConnector> _moveConnectors = new HashSet<IMoveConnector>();
		private readonly HashSet<IMoveConnector> _parentConnectors = new HashSet<IMoveConnector>();
		private readonly HashSet<string> _moveSet = new HashSet<string>();

		public void Init(IMoveConnecteable connectedElement, ICharacterCollider characterCollider)
		{
			_connectedElement = connectedElement;
			_characterCollider = characterCollider;
		}

		public bool HaveCloseCycleConnection()
		{
			return _moveConnectors.Any(moveConnector => moveConnector.HaveConnector(this));
		}

		public bool HaveConnector(IMoveConnector wantedConnector)
		{
			return _moveConnectors.Any((moveConnector) => moveConnector == wantedConnector);
		}

		public bool CheckCycleConnection()
		{
			return _moveConnectors.Any(moveConnector => moveConnector.HaveChildConnector(this));
		}

		public bool HaveChildConnector(IMoveConnector wantedConnector)
		{
			return _moveConnectors.Any((moveConnector) =>
			{
				if (moveConnector == wantedConnector) return true;
				return moveConnector.HaveChildConnector(wantedConnector);
			});
		}

		public bool CouldCycleMove()
		{
			_moveSet.Clear();
			RegisterAttempt(_moveSet);
			return _moveConnectors.All((moveConnector) => moveConnector.CheckCycleMovePossibility(_moveSet, this));
		}

		public bool RegisterAttempt(HashSet<string> moveSet)
		{
			var prevLength = moveSet.Count;
			var numberOfOccupiedCells = _connectedElement.RegisterNextMove(moveSet);
			return moveSet.Count == prevLength + numberOfOccupiedCells;
		}

		public void ConnectTo(IMoveConnector moveConnector)
		{
			_parentConnectors.Add(moveConnector);
			moveConnector.AddConnection(this);
		}

		public bool CheckCycleMovePossibility(HashSet<string> moveSet, IMoveConnector initMoveConnector)
		{
			if (initMoveConnector == this) return true;
			if (!RegisterAttempt(moveSet)) return false;
			return _moveConnectors.All((moveConnector) => moveConnector.CheckCycleMovePossibility(moveSet, initMoveConnector));
		}

		public void OnMoveMade(bool moved)
		{
			foreach (var connectedBattleUnit in _moveConnectors)
			{
				connectedBattleUnit.OnConnectionMove(this, moved);
			}
			_moveConnectors.Clear();
		}

		public void OnConnectionMove(IMoveConnector parentConnector, bool moved)
		{
			_parentConnectors.Remove(parentConnector);
			if (_parentConnectors.Count != 0) return;
			_connectedElement?.OnConnectionMove(moved);
		}

		public void AddConnection(IMoveConnector moveConnector)
		{
			_moveConnectors.Add(moveConnector);
		}
	}
}