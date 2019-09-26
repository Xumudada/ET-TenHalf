using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[Event(EventIdType.LoadSceneRoom)]
	public class LoadSceneRoomEvent: AEvent
	{
		public override void Run()
		{
			SceneUtil.LoadScene(SceneType.Room, () =>
			{
				UIUtil.CloseAllPanel();
				UIUtil.OpenPanel<RoomPanelEntity>(UIType.RoomPanel);
			}).Coroutine();
		}
	}
}
