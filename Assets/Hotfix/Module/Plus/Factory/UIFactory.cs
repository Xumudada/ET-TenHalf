using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
	public static class UIFactory
	{
		public static UI Create<T>(string uiName) where T : Component, new()
		{
			try
			{
				ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
				resourcesComponent.LoadBundle(uiName.StringToAB());
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(uiName.StringToAB(), uiName);
				GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

				UI ui = ComponentFactory.Create<UI, string, GameObject>(uiName, gameObject);
				ui.AddComponent<T>();

				return ui;
			}
			catch (Exception e)
			{
				Log.Error(e);
				return null;
			}
		}
	}
}