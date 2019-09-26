using ETHotfix;
using UnityEngine;

namespace ETModel
{
	public static class GameExtension
	{
		public static bool ToBool(this byte self)
		{
			if (self == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
