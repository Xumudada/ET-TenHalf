using ETModel;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace ETHotfix
{
	public enum NumericType
	{
		Max = 10000,			// Warning: 永远不能超过这个值

		Speed = 1000,
		SpeedBase = Speed * 10 + 1,
		SpeedAdd = Speed * 10 + 2,
		SpeedPct = Speed * 10 + 3,
		SpeedFinalAdd = Speed * 10 + 4,
		SpeedFinalPct = Speed * 10 + 5,

		Hp = 1001,
		HpBase = Hp * 10 + 1,

		MaxHp = 1002,
		MaxHpBase = MaxHp * 10 + 1,
		MaxHpAdd = MaxHp * 10 + 2,
		MaxHpPct = MaxHp * 10 + 3,
		MaxHpFinalAdd = MaxHp * 10 + 4,
		MaxHpFinalPct = MaxHp * 10 + 5,
	}

	public class NumericEntity : Entity
	{
		public readonly Dictionary<int, float> NumericDic = new Dictionary<int, float>();

		public float Get(NumericType nt)
		{
			return GetByKey((int)nt);
		}

		public void Set(NumericType nt, float value)
		{
			this[nt] = value;
		}

		public float this[NumericType nt]
		{
			get => Get(nt);
			set
			{
				float v = GetByKey((int)nt);
				if (v == value)
				{
					return;
				}

				NumericDic[(int)nt] = value;
				Update(nt);
			}
		}

		private float GetByKey(int key)
		{
			float value = 0;
			this.NumericDic.TryGetValue(key, out value);
			return value;
		}

		public void Update(NumericType nt)
		{
			if (nt > NumericType.Max)
			{
				Debug.LogError($"当前\"NumericType.{Enum.GetName(typeof(NumericType), nt)}\"的值: 超出了10000!");
				return;
			}

			int final = (int)nt / 10;
			int bas = final * 10 + 1;
			int add = final * 10 + 2;
			int pct = final * 10 + 3;
			int finalAdd = final * 10 + 4;
			int finalPct = final * 10 + 5;

			NumericDic[final] = ((GetByKey(bas) + GetByKey(add)) * (100 + GetByKey(pct)) / 100 + GetByKey(finalAdd)) * (100 + GetByKey(finalPct)) / 100;
			Game.EventSystem.Run(EventIdType.NumericChange, Id, final, NumericDic[final]);
		}
	}
}
