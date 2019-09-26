using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ETHotfix
{
	[ObjectSystem]
	public class TipPanelEntityAwakeSystem : AwakeSystem<TipPanelEntity>
	{
		public override void Awake(TipPanelEntity self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class TipPanelEntityStartSystem : StartSystem<TipPanelEntity>
	{
		public override void Start(TipPanelEntity self)
		{
			self.Start();
		}
	}

	[ObjectSystem]
	public class TipPanelEntityUpdateSystem : UpdateSystem<TipPanelEntity>
	{
		public override void Update(TipPanelEntity self)
		{
			self.Update();
		}
	}

	[ObjectSystem]
	public class TipPanelEntityDestroySystem : DestroySystem<TipPanelEntity>
	{
		public override void Destroy(TipPanelEntity self)
		{
			self.OnDestroy();
		}
	}

	public class TipPanelEntity : Entity
	{
		private ReferenceCollector mCollector;
		private Text tipText;

		public void Awake()
		{
			mCollector = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			tipText = mCollector.Get<GameObject>("TipText").GetComponent<Text>();
		}

		public void Start()
		{
			// 动画
			tipText.transform.DOLocalMoveY(Screen.height / 2.5f, 1f).From(Screen.height / 7f);
			tipText.DoFade(500, new Color(1f, 1f, 1f, 0.3f), Color.white).Coroutine();

			// 自动关闭
			this.Delay(1500, () =>
			{
				UIUtil.ClosePanel("TipPanel");
			}).Coroutine();
		}

		public void Update()
		{

		}

		public void OnDestroy()
		{

		}

		public void SetTip(string tip)
		{
			tipText.text = tip;
		}

		public static void ShowTip(string tip)
		{
			UIUtil.OpenPanel<TipPanelEntity>(UIType.TipPanel)?.SetTip(tip);
		}
	}
}
