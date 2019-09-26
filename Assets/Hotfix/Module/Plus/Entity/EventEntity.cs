using ETModel;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ETHotfix
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

	public class EventEntity : Entity
	{
		public void Awake()
		{

		}

		public void Start()
		{

		}

		public void Update()
		{
			ETModel.EventEntity modelEventEntity = ETModel.Game.Scene.GetComponent<ETModel.EventEntity>();
			if (modelEventEntity == null) return;

			List<EventInfo> eventInfos = modelEventEntity.GetAll();
			for (int i = 0; i < eventInfos.Count; i++)
			{
				EventMgr.Send(eventInfos[i].key, eventInfos[i].args);
			}

			modelEventEntity.Clear();
		}

		public void OnDestroy()
		{

		}
	}
}
