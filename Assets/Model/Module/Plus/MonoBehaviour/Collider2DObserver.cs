using ETHotfix;
using UnityEngine;

namespace ETModel
{
	public class Collider2DObserver : MonoBehaviour
	{
		private void OnCollisionEnter2D(Collision2D collision)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnCollisionEnter2D, gameObject, collision);
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnCollisionStay2D, gameObject, collision);
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnCollisionExit2D, gameObject, collision);
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnTriggerEnter2D, gameObject, collider);
		}

		private void OnTriggerStay2D(Collider2D collider)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnTriggerStay2D, gameObject, collider);
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			Game.Scene.GetComponent<EventEntity>().Add(EventKey.OnTriggerExit2D, gameObject, collider);
		}
	}
}
