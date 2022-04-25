using System.Collections.Generic;

namespace LordAmbermaze.Core
{
	public interface IMoveConnecteable
	{
		void OnConnectionMove(bool moved);
		int RegisterNextMove(HashSet<string> moveSet);
	}
}