using System.Collections.Generic;
using LordAmbermaze.Core;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public class TileDetectorRaycast : MonoBehaviour, ITileDetector
	{
		[SerializeField]
		private LayerMask _layerMask;
		private readonly Dictionary<string, SlotType> _collDictionary = new Dictionary<string, SlotType>()
		{
			{ Tags.Wall, SlotType.Wall },
			{ Tags.Lake, SlotType.Lake },
            { Tags.Toxic, SlotType.Lake },
			{ Tags.Pit, SlotType.Pit },
			{ Tags.Untagged, SlotType.Empty }
		};

        private readonly HashSet<string> _obstaclesTags = new HashSet<string>() { Tags.Wall, Tags.Lake };

		public SlotType GetTileSlotType(Vector2 coordinate)
		{
			Collider2D[] collides = Physics2D.OverlapPointAll(coordinate, _layerMask);
			if (collides.Length < 1) return SlotType.Empty;
			var coll = collides[0];
			if (_collDictionary.ContainsKey(coll.tag))
			{
				return _collDictionary[coll.tag];
			}

			return SlotType.Empty;
		}

		public TileData GetTileData(Vector2 coordinate)
		{
			Collider2D[] collides = Physics2D.OverlapPointAll(coordinate, _layerMask);
			var slotType = SlotType.Empty;
			if (collides.Length < 1) return new TileData(coordinate, null, slotType);
			Collider2D collider = null;
			foreach (var coll in collides)
			{
				collider = coll;
				if (_collDictionary.ContainsKey(coll.tag))
				{
					slotType = _collDictionary[coll.tag];
					break;
				}
				
			}
			IMoveConnector battleUnitConnector = null;
			var charCollider = collider.gameObject.GetComponent<ICharacterCollider>();
			if (charCollider != null)
			{
				battleUnitConnector = charCollider.GetMoveable();
			}

			return new TileData(coordinate, battleUnitConnector, slotType);
		}

 

		public T CheckComponent<T>(Vector2 coordinate) where T : class
		{
			Collider2D[] collides = Physics2D.OverlapPointAll(coordinate, _layerMask);
			foreach (var coll in collides)
			{
				var component = coll.GetComponent<T>();
				if (component != null)
				{
					return component;
				}
			}

			return null;
		}
	}
}