using ETHotfix;
using UnityEngine;
using System;

namespace ETModel
{
	public class AnimatorObserver : MonoBehaviour
	{
		public void Send(string msg)
		{
			Game.Scene.GetComponent<EventEntity>().Add((EventKey)(Enum.Parse(typeof(EventKey), msg)));
		}
	}
}
