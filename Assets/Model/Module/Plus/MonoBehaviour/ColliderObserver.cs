using ETHotfix;
using UnityEngine;

namespace ETModel
{
	public class ColliderObserver : MonoBehaviour
	{
		private void OnCollisionEnter(Collision collision)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnCollisionEnter, gameObject, collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnCollisionStay, gameObject, collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnCollisionExit, gameObject, collision);
		}

		private void OnTriggerEnter(Collider collider)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnTriggerEnter, gameObject, collider);
		}

		private void OnTriggerStay(Collider collider)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnTriggerStay, gameObject, collider);
		}

		private void OnTriggerExit(Collider collider)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnTriggerExit, gameObject, collider);
		}
	}
}
