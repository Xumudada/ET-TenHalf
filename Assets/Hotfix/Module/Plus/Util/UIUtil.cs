using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
	public static class UIUtil
	{
		private static List<string> uiNameList = new List<string>();

		public static T OpenPanel<T>(string uiName) where T : Component, new()
		{
			if (uiNameList.Contains(uiName))
			{
				return null;
			}
			uiNameList.Add(uiName);

			UI ui = UIFactory.Create<T>(uiName);
			Game.Scene.GetComponent<UIComponent>().Add(ui);
			return ui.GetComponent<T>();
		}

		public static T GetPanel<T>(string uiName) where T : Component, new()
		{
			return Game.Scene.GetComponent<UIComponent>().Get(uiName).GetComponent<T>();
		}

		public static void ClosePanel(string uiName)
		{
			if (uiNameList.Contains(uiName) == false) return;

			uiNameList.Remove(uiName);
			Game.Scene.GetComponent<UIComponent>().Remove(uiName);
			ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(uiName.StringToAB());
		}

		public static void CloseAllPanel()
		{
			for (int i = 0; i < uiNameList.Count; i++)
			{
				ClosePanel(uiNameList[i]);
			}
		}
	}
}
