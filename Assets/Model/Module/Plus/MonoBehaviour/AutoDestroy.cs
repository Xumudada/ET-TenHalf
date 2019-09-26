using ETHotfix;
using UnityEngine;

namespace ETModel
{
	public class AutoDestroy : MonoBehaviour
	{
		private void Awake()
		{
			Destroy(gameObject);
		}
	}
}
