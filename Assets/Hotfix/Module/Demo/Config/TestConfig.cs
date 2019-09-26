using ETModel;

namespace ETHotfix
{
	[Config((int)(AppType.ClientH))]
	public partial class TestConfigCategory : ACategory<TestConfig>
	{

	}

	public class TestConfig: IConfig
	{
		public long Id { get; set; }
		public string Useage;
		public string Desc;
	}
}
