using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[Event(EventIdType.LoadSceneLogin)]
	public class LoadSceneLoginEvent : AEvent
	{
		public override void Run()
		{
			SceneUtil.LoadScene(SceneType.Login, () =>
			{
				UIUtil.CloseAllPanel();
				UIUtil.OpenPanel<LoginPanelEntity>(UIType.LoginPanel);
			}).Coroutine();
		}
	}
}
