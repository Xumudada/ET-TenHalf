using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[Event(EventIdType.LoadSceneLobby)]
	public class LoadSceneLobbyEvent: AEvent
	{
		public override void Run()
		{
			SceneUtil.LoadScene(SceneType.Lobby, () =>
			{
				UIUtil.CloseAllPanel();
				UIUtil.OpenPanel<LobbyPanelEntity>(UIType.LobbyPanel);
			}).Coroutine();
		}
	}
}
