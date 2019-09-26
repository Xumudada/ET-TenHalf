using ETHotfix;
using UnityEngine;
using KBEngine;

namespace ETModel
{
	public static class Constant
	{
		public const int ROOM_MAX_PLAYER = 2;

		public static Account Player
		{
			get
			{
				return KBEngineApp.app.player() as Account;
			}
		}

		public static EntityBaseEntityCall_AccountBase PlayerBase
		{
			get
			{
				return Player.baseEntityCall;
			}
		}

		public static EntityCellEntityCall_AccountBase PlayerCell
		{
			get
			{
				return Player.cellEntityCall;
			}
		}

		public const string LoadCacheEntityList = "LoadCacheEntityList";
	}
}
