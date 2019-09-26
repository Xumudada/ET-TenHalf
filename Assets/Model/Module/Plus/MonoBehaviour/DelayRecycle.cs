using ETHotfix;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ETModel
{
	public class DelayRecycle : MonoBehaviour
	{
		public float delay = 0f;

		private void Awake()
		{
			StartCoroutine(Delay());
		}

		private IEnumerator Delay()
		{
			yield return new WaitForSeconds(delay);
			GameObjectPool.Recycle(gameObject);
		}
	}
}
