using ETModel;

namespace ETHotfix
{
	public static class ConfigUtil
	{
		public static T GetConfig<T>(int id) where T : IConfig
		{
			return (T)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(T), id);
		}

		public static string GetJson<T>(int id) where T : IConfig
		{
			return JsonHelper.ToJson(GetConfig<T>(id));
		}
	}
}
