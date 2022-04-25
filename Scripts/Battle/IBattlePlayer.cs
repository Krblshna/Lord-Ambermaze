using LordAmbermaze.Core;

namespace LordAmbermaze.Battle
{
	public interface IBattlePlayer
	{
		ICharacterCollider Init(IBattleManager battleManager);
	}
}