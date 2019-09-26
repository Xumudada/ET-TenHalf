using ETHotfix;
using UnityEngine;

namespace ETModel
{
	public class FXDestroy : MonoBehaviour
	{
		public ParticleSystem particle;

		private void Update()
		{
			if (particle.IsAlive() == false)
			{
				Destroy(gameObject);
			}
		}
	}
}
