using UnityEngine;
using UnityEngine.UI;
using System;

namespace ETModel
{
	public static class PlusExtension
	{
		#region Text

		/// <summary>
		/// 颜色渐变
		/// </summary>
		/// <param name="delayMs">延迟的ms</param>
		/// <param name="from">原来的颜色</param>
		/// <param name="target">目标颜色</param>
		public static async ETVoid DoFade(this Text self, long delayMs, Color from, Color target)
		{
			long frameCount = delayMs / 20;
			self.color = from;
			for (long i = 0; i < frameCount; i++)
			{
				if (self == null) return;

				self.color = (from + target) * i / frameCount;
				await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(20);
			}
		}

		#endregion

		#region GameObject

		public static void Destroy(this GameObject self, float delay = 0f)
		{
			GameObject.Destroy(self, delay);
		}

		public static GameObject Instantiate(this GameObject self)
		{
			return GameObject.Instantiate(self);
		}

		public static GameObject Identify(this GameObject self)
		{
			self.Position(Vector3.zero);
			self.Rotation(Quaternion.identity);
			self.Scale(Vector3.one);
			return self;
		}

		public static GameObject Position(this GameObject self, Vector3 pos)
		{
			self.transform.position = pos;
			return self;
		}

		public static GameObject Enable(this GameObject self)
		{
			self.SetActive(true);
			return self;
		}

		public static GameObject Disable(this GameObject self)
		{
			self.SetActive(false);
			return self;
		}

		public static GameObject Parent(this GameObject self, GameObject parent)
		{
			self.transform.SetParent(parent.transform);
			return self;
		}

		public static GameObject Rotation(this GameObject self, Quaternion rotation)
		{
			self.transform.rotation = rotation;
			return self;
		}

		public static GameObject Scale(this GameObject self, Vector3 scale)
		{
			self.transform.localScale = scale;
			return self;
		}

		public static GameObject Name(this GameObject self, string name)
		{
			self.name = name;
			return self;
		}

		#endregion

		#region Vector3

		public static Vector2 ToVector2(this Vector3 self)
		{
			return new Vector2(self.x, self.y);
		}

		public static Vector3 ClearX(this Vector3 self)
		{
			self.x = 0f;
			return self;
		}

		public static Vector3 ClearY(this Vector3 self)
		{
			self.y = 0f;
			return self;
		}

		public static Vector3 ClearZ(this Vector3 self)
		{
			self.z = 0f;
			return self;
		}

		#endregion
	}
}
