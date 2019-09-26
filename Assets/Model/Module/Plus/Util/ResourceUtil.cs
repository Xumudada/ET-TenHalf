namespace ETModel
{
	public static class ResourceUtil
	{
		public static T Load<T>(string assetName) where T : class
		{
			ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
			resourcesComponent.LoadBundle(assetName.StringToAB());
			return resourcesComponent.GetAsset(assetName.StringToAB(), assetName) as T;
		}

		public static void Unload(string assetName)
		{
			ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(assetName.StringToAB());
		}
	}
}
