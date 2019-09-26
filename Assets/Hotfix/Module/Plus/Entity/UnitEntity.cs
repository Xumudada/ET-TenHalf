using ETModel;
using UnityEngine;
using System.Collections.Generic;

namespace ETHotfix
{
	[ObjectSystem]
	public class UnitEntityAwakeSystem : AwakeSystem<UnitEntity>
	{
		public override void Awake(UnitEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class UnitEntityStartSystem : StartSystem<UnitEntity>
	{
		public override void Start(UnitEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class UnitEntityUpdateSystem : UpdateSystem<UnitEntity>
	{
		public override void Update(UnitEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class UnitEntityDestroySystem : DestroySystem<UnitEntity>
	{
		public override void Destroy(UnitEntity self)
		{
			self.OnDestroy();
		}
	}

	public class UnitEntity : Entity
	{
		private readonly Dictionary<long, Entity> idUnits = new Dictionary<long, Entity>();

		public void Awake()
		{

		}

		public void Start()
		{

		}

		public void Update()
		{

		}

		public void OnDestroy()
		{

		}

		public override void Dispose()
		{
			if (IsDisposed) return;
			base.Dispose();

			idUnits.Clear();
		}

		public void Add(Entity unit)
		{
			idUnits.Add(unit.Id, unit);
			unit.Parent = this;
		}

		public T Get<T>(long id) where T : Entity
		{
			Entity unit;
			this.idUnits.TryGetValue(id, out unit);
			return unit as T;
		}

		public void Remove(long id)
		{
			Entity unit;
			idUnits.TryGetValue(id, out unit);
			if (unit != null)
			{
				unit?.Dispose();
				idUnits.Remove(id);
				if (unit.GameObject)
				{
					GameObject.Destroy(unit.GameObject);
				}
			}
			else
			{
				Debug.LogError($"不存在Unit: {id}");
			}
		}

		public void RemoveAll()
		{
			foreach (var id in idUnits.Keys)
			{
				GameObject.Destroy(idUnits[id].GameObject);
				idUnits[id].Dispose();
			}
			idUnits.Clear();
		}
	}
}
