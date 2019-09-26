using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class GameEntityAwakeSystem : AwakeSystem<GameEntity>
	{
		public override void Awake(GameEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class GameEntityStartSystem : StartSystem<GameEntity>
	{
		public override void Start(GameEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class GameEntityUpdateSystem : UpdateSystem<GameEntity>
	{
		public override void Update(GameEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class GameEntityDestroySystem : DestroySystem<GameEntity>
	{
		public override void Destroy(GameEntity self)
		{
			self.Deregister();
			self.OnDestroy();
		}
	}

	public class GameEntity : Entity
	{
		public void Awake()
		{

		}

		public void Start()
		{
			this.Register(EventKey.onEnterRoom, OnEvent);
			this.Register(EventKey.onLeaveRoom, OnEvent);
		}

		public void Update()
		{

		}

		public void OnDestroy()
		{

		}

		public void OnEvent(EventKey key, object[] args)
		{
			if (key == EventKey.onEnterRoom)
			{
				Game.EventSystem.Run(EventIdType.LoadSceneRoom);
			}
			else if (key == EventKey.onLeaveRoom)
			{
				Game.EventSystem.Run(EventIdType.LoadSceneLobby);
			}
		}
	}
}
