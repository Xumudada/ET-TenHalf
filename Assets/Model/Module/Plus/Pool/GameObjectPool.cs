using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ETModel
{
	public static class GameObjectPool
	{
		private class GOInfo
		{
			public string name;
			public GameObject prefab;
		}

		private static Dictionary<GOInfo, Queue<GameObject>> goTable = new Dictionary<GOInfo, Queue<GameObject>>();

		public static GameObject Allocate(string name)
		{
			GOInfo info = GetGOInfo(name);
			Queue<GameObject> queue = goTable[info];
			if (queue.Count > 0)
			{
				return queue.Dequeue().Enable();
			}
			else
			{
				return info.prefab.Instantiate().Name(info.prefab.name);
			}
		}

		public static void Recycle(GameObject go)
		{
			if (go)
			{
				go.Disable();
				GOInfo info = GetGOInfo(go.name);
				goTable[info].Enqueue(go);
			}
		}

		private static GOInfo GetGOInfo(string name)
		{
			var iter = goTable.GetEnumerator();
			while (iter.MoveNext())
			{
				GOInfo info = iter.Current.Key;
				if (info.name == name)
				{
					return info;
				}
			}

			// 没有就创建
			GOInfo goInfo = new GOInfo() { name = name, prefab = ResourceUtil.Load<GameObject>(name) };
			Queue<GameObject> queue = new Queue<GameObject>();
			goTable.Add(goInfo, queue);
			return goInfo;
		}
	}
}
