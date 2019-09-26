using ETModel;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ETHotfix
{
	public delegate void OnEvent(EventKey key, object[] args);

	public class EventMgr
	{
		public static Dictionary<EventKey, Dictionary<object, OnEvent>> eventTable = new Dictionary<EventKey, Dictionary<object, OnEvent>>();

		public static void Register(EventKey key, object obj, OnEvent onEvent)
		{
			if (eventTable.ContainsKey(key) == false)
			{
				Dictionary<object, OnEvent> objectTable = new Dictionary<object, OnEvent>();
				eventTable.Add(key, objectTable);
			}

			if (eventTable[key].ContainsKey(obj) == false)
			{
				eventTable[key].Add(obj, null);
			}

			eventTable[key][obj] += onEvent;
		}

		public static void Deregister(object obj)
		{
			foreach (var item in eventTable)
			{
				item.Value.Remove(obj);
			}
		}

		public static void Send(EventKey key, params object[] args)
		{
			if (eventTable.ContainsKey(key) == false || eventTable[key] == null)
			{
				return;
			}

			Dictionary<object, OnEvent> objTable = eventTable[key];
			List<object> objList = objTable.Keys.ToList();
			for (int i = 0; i < objList.Count; i++)
			{
				if (objTable.ContainsKey(objList[i]))
				{
					objTable[objList[i]]?.Invoke(key, args);
				}
			}
		}
	}
}
