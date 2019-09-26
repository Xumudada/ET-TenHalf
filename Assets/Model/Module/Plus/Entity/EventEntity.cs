using ETHotfix;
using UnityEngine;
using System.Collections.Generic;
using KBEngine;
using System;
using Entity = KBEngine.Entity;

namespace ETModel
{
	[ObjectSystem]
	public class EventEntityAwakeSystem : AwakeSystem<EventEntity>
	{
		public override void Awake(EventEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class EventEntityStartSystem : StartSystem<EventEntity>
	{
		public override void Start(EventEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class EventEntityUpdateSystem : UpdateSystem<EventEntity>
	{
		public override void Update(EventEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class EventEntityDestroySystem : DestroySystem<EventEntity>
	{
		public override void Destroy(EventEntity self)
		{
			self.OnDestroy();
		}
	}

	public struct EventInfo
	{
		public EventKey key;
		public object[] args;

		public EventInfo(EventKey key, object[] args)
		{
			this.key = key;
			this.args = args;
		}
	}

	public class EventEntity : Entity
	{
		private List<EventInfo> eventInfoList = new List<EventInfo>();

		private List<KBEngine.Entity> cacheEntityList = new List<KBEngine.Entity>();

		public void Awake()
		{

		}

		public void Start()
		{
			KBEngine.Event.registerOut(EventOutTypes.onCreateAccountResult, this, EventOutTypes.onCreateAccountResult);
			KBEngine.Event.registerOut(EventOutTypes.onLoginFailed, this, EventOutTypes.onLoginFailed);
			KBEngine.Event.registerOut(EventOutTypes.onEnterWorld, this, EventOutTypes.onEnterWorld);
			KBEngine.Event.registerOut(EventOutTypes.onLeaveWorld, this, EventOutTypes.onLeaveWorld);
			KBEngine.Event.registerOut(Constant.LoadCacheEntityList, this, Constant.LoadCacheEntityList);
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
		}

		public void Add(EventKey key, params object[] args)
		{
			eventInfoList.Add(new EventInfo(key, args));
		}

		public List<EventInfo> GetAll()
		{
			return eventInfoList;
		}

		public void Clear()
		{
			eventInfoList.Clear();
		}

		public void onCreateAccountResult(UInt16 retcode, byte[] datas)
		{
			Add(EventKey.onCreateAccountResult, retcode);
		}

		public void onLoginFailed(UInt16 retcode, byte[] datas)
		{
			Add(EventKey.onLoginFailed, retcode);
		}

		public void onEnterWorld(KBEngine.Entity entity)
		{
			cacheEntityList.Add(entity);
			Add(EventKey.onEnterWorld, entity);
		}

		public void onLeaveWorld(KBEngine.Entity entity)
		{
			cacheEntityList.Remove(entity);
			Add(EventKey.onLeaveWorld, entity);
		}

		public void LoadCacheEntityList()
		{
			foreach (var entity in cacheEntityList)
			{
				Add(EventKey.onEnterWorld, entity);
			}
		}
	}
}
