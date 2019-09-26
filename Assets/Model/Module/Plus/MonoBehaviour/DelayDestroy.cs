using ETHotfix;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ETModel
{
	public class DelayDestroy : MonoBehaviour
	{
		public float delay = 0f;

		private void Awake()
		{
			Destroy(gameObject, delay);
		}
	}
}
