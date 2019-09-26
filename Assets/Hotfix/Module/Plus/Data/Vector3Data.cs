using ETModel;
using UnityEngine;

namespace ETHotfix
{
	public class Vector3Data
	{
		public float x;
		public float y;
		public float z;

		public Vector3Data(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3 ToVector3()
		{
			return new Vector3(x, y, z);
		}
	}
}
