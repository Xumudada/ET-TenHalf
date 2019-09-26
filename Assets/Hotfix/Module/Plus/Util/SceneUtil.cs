using ETModel;
using System;

namespace ETHotfix
{
	public static class SceneUtil
	{
		public static async ETVoid LoadScene(string sceneName, Action onLoaded = null)
		{
			Game.Scene.GetComponent<UnitEntity>().RemoveAll();
			EventMgr.Send(EventKey.StartLoadScene);

			// 加载场景资源
			await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync(sceneName.StringToAB());

			while (ETModel.Game.Scene.GetComponent<SceneChangeComponent>() != null)
			{
				await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
			}

			using (SceneChangeComponent sceneChangeComponent = ETModel.Game.Scene.AddComponent<SceneChangeComponent>())
			{
				await sceneChangeComponent.ChangeSceneAsync(sceneName);
			}

			EventMgr.Send(EventKey.EndLoadScene);
			onLoaded?.Invoke();
		}
	}
}
