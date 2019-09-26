using ETHotfix;
using UnityEngine;
using ETModel;

namespace KBEngine
{
	public class Account : AccountBase
	{
		public override void __init__()
		{
			base.__init__();

			if (isPlayer())
			{
				Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnLoginSuccessfully);
			}
		}

		public override void onGetCell()
		{
			base.onGetCell();

			if (isPlayer())
			{
				Game.Scene.GetComponent<EventEntity>().Add(EventKey.onEnterRoom);
			}
		}

		public override void onPlayernameBaseChanged(string oldValue)
		{
			base.onPlayernameBaseChanged(oldValue);

			Game.Scene.GetComponent<EventEntity>().Add(EventKey.onPlayernameBaseChanged, playernameBase);
		}

		public override void onGoldBaseChanged(uint oldValue)
		{
			base.onGoldBaseChanged(oldValue);

			Game.Scene.GetComponent<EventEntity>().Add(EventKey.onGoldBaseChanged, goldBase);
		}

		public override void onIsReadyChanged(byte oldValue)
		{
			base.onIsReadyChanged(oldValue);

			Game.Scene.GetComponent<EventEntity>().Add(EventKey.onIsReadyChanged, id, isReady.ToBool());
		}

		public override void onJoinRoom(byte retcode)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.onJoinRoom, retcode);
		}

		public override void onLeaveRoom()
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.onLeaveRoom);
		}
	}
}
