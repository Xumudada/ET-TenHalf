using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[ObjectSystem]
	public class ResHoldEntityAwakeSystem : AwakeSystem<ResHoldEntity>
	{
		public override void Awake(ResHoldEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class ResHoldEntityStartSystem : StartSystem<ResHoldEntity>
	{
		public override void Start(ResHoldEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class ResHoldEntityUpdateSystem : UpdateSystem<ResHoldEntity>
	{
		public override void Update(ResHoldEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class ResHoldEntityDestroySystem : DestroySystem<ResHoldEntity>
	{
		public override void Destroy(ResHoldEntity self)
		{
			self.OnDestroy();
		}
	}

	[HideInHierarchy]
	public class ResHoldEntity : Entity
	{
		private ReferenceCollector Collector { get; set; }

		public void Awake()
		{
			GameObject = ResourceUtil.Load<GameObject>("ResHold").Instantiate();
			GameObject.AddComponent<ComponentView>().Component = this;
			Collector = GameObject.GetComponent<ReferenceCollector>();
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

		public T Get<T>(string key) where T : class
		{
			return Collector.Get<T>(key);
		}
	}
}
