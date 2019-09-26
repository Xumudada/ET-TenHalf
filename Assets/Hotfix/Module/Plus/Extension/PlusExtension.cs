using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ETHotfix
{
	public static class PlusExtension
	{
		#region Component

		public static async ETVoid Delay(this Component self, long delayMs, Action callback)
		{
			await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(delayMs);
			if (self != null)
			{
				callback.Invoke();
			}
		}

		#endregion

		#region object

		public static void Register(this object self, EventKey eventType, OnEvent callback)
		{
			EventMgr.Register(eventType, self, callback);
		}

		public static void Deregister(this object self)
		{
			EventMgr.Deregister(self);
		}

		public static void Send(this object self, EventKey eventType, params object[] args)
		{
			EventMgr.Send(eventType, args);
		}

		#endregion
	}
}
