using AZ.Core;
using LordAmbermaze.Battle;
using UnityEngine;

namespace LordAmbermaze.Core
{
	public interface IAttacker
	{
		void Attack(Vector2 attakDirection, Func onAttackMoment=null);
		void Init(IBattleManager battleManager, Transform moveTransform);
		void Attack(ICharacterCollider charCollider);
	}
}