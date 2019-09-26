using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using ETModel;
using System.Threading.Tasks;

namespace ETPlus
{
	public class AutoGenerateCodeEditor : UnityEditor.AssetModificationProcessor
	{
		private enum ScriptType
		{
			Other,
			Component,
			Event,
			Config,
			Factory
		}

		/// <summary>
		/// 即将创建Asset时
		/// </summary>
		/// <param name="assetName">创建的Asset名字</param>
		private static void OnWillCreateAsset(string path)
		{
			path = path.Replace(".meta", "");

			// 如果不是 CSharp
			if (path.EndsWith(".cs") == false)
			{
				return;
			}

			// 排除导入的脚本 和 获取类名
			string text = File.ReadAllText(path);
			string className = GetClassName(text);
			if (className == null)
			{
				return;
			}

			// 获取脚本类型， 这决定生成的脚本样式
			ScriptType scriptType = GetScriptType(className);

			string scriptNamespace = "";
			string eachNamespace = "";
			if (path.StartsWith("Assets/Hotfix/"))
			{
				scriptNamespace = "ETHotfix";
				eachNamespace = "ETModel";
			}
			else if (path.StartsWith("Assets/Model/"))
			{
				scriptNamespace = "ETModel";
				eachNamespace = "ETHotfix";
			}
			else
			{
				return;
			}

			// 自定义脚本
			using (FileStream fs = new FileStream(path, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
				{
					// 起头
					if (scriptType == ScriptType.Component)
					{
						string parentClassName = className.EndsWith("Component") ? "Component" : "Entity";
						sw.Write(
$"using {eachNamespace};" +
@"
using UnityEngine;

namespace " + scriptNamespace +
@"
{
	[ObjectSystem]
" +
		$"	public class {className}AwakeSystem : AwakeSystem<{className}>" +
		@"
	{
" +
		$"		public override void Awake({className} self)" +
		@"
		{
			self.Awake();
		}
	}

	[ObjectSystem]
" +
		$"	public class {className}StartSystem : StartSystem<{className}>" +
		@"
	{
" +
		$"		public override void Start({className} self)" +
		@"
		{
			self.Start();
		}
	}

	[ObjectSystem]
" +
		$"	public class {className}UpdateSystem : UpdateSystem<{className}>" +
		@"
	{
" +
		$"		public override void Update({className} self)" +
		@"
		{
			self.Update();
		}
	}

	[ObjectSystem]
" +
		$"	public class {className}DestroySystem : DestroySystem<{className}>" +
		@"
	{
" +
		$"		public override void Destroy({className} self)" +
		@"
		{
			self.Deregister();
			self.OnDestroy();
		}
	}

" +
		$"	public class {className} : {parentClassName}" +
		@"
	{
		public void Awake()
		{

		}

		public void Start()
		{

		}

		public void Update()
		{

		}

		public void OnDestroy()
		{

		}

		public void OnEvent(EventKey key, object[] args)
		{
			
		}
	}
}
");
					}
					else if (scriptType == ScriptType.Event)
					{
						string eventName = className.Replace("Event", "");

						// 添加EventIdType
						AddEventIdType(eventName, scriptNamespace);

						// 生成事件脚本
						sw.WriteLine($"using {eachNamespace};");
						sw.WriteLine("using UnityEngine;");
						sw.WriteLine();
						sw.WriteLine($"namespace {scriptNamespace}");
						sw.WriteLine("{");
						sw.WriteLine($"	[Event(EventIdType.{eventName})]");
						sw.WriteLine($"	public class {className}: AEvent");
						sw.Write(
@"	{
		public override void Run()
		{

		}
	}
}
");
					}
					else if (scriptType == ScriptType.Config)
					{
						string configName = className.Replace("Config", "");

						// 生成事件脚本
						sw.WriteLine($"using {eachNamespace};");
						sw.WriteLine();
						sw.WriteLine($"namespace {scriptNamespace}");
						sw.WriteLine("{");
						sw.WriteLine("	[Config((int)(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map))]");
						sw.WriteLine($"	public partial class {className}Category : ACategory<{className}>");
						sw.WriteLine("	{");
						sw.WriteLine();
						sw.WriteLine("	}");
						sw.WriteLine();
						sw.WriteLine($"	public class {className}: IConfig");
						sw.Write(
@"	{
		public long Id { get; set; }
	}
}
"
);
						CreateConfigTxt(className).Coroutine();
					}
					else if (scriptType == ScriptType.Factory)
					{
						sw.WriteLine($"using {eachNamespace};");
						sw.WriteLine("using UnityEngine;");
						sw.WriteLine();
						sw.WriteLine($"namespace {scriptNamespace}");
						sw.WriteLine("{");
						sw.WriteLine($"	public static class {className}");
						sw.WriteLine("	{");
						sw.WriteLine($"		public static {className.Replace("Factory", "")} Create()");
						sw.WriteLine("		{");
						sw.WriteLine("			return null;");
						sw.WriteLine("		}");
						sw.WriteLine("	}");
						sw.WriteLine("}");
					}
					else
					{
						sw.WriteLine($"using {eachNamespace};");
						sw.WriteLine("using UnityEngine;");
						sw.WriteLine();
						sw.WriteLine($"namespace {scriptNamespace}");
						sw.WriteLine("{");
						sw.WriteLine($"	public class {className}");
						sw.WriteLine("	{");
						sw.WriteLine();
						sw.WriteLine("	}");
						sw.WriteLine("}");
					}
				}
			}

			AssetDatabase.Refresh();
		}

		/// <summary>
		/// 创建配置文本
		/// </summary>
		/// <param name="className">类名</param>
		private static async ETVoid CreateConfigTxt(string className)
		{
			// 写入配置文件
			string configPath = $"Assets/Res/Config/{className}.txt";
			using (FileStream configFile = new FileStream(configPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				using (StreamWriter configWriter = new StreamWriter(configFile, System.Text.Encoding.UTF8))
				{
					configWriter.Write(@"{""Id"" : 1}");
				}
			}

			// 为了保证创建并刷新配置文件
			await Task.Delay(500);

			// 加入到Config.prefab
			GameObject configPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Bundles/Independent/Config.prefab");
			ReferenceCollector collector = configPrefab.GetComponent<ReferenceCollector>();
			TextAsset configTxt = AssetDatabase.LoadAssetAtPath<TextAsset>(configPath);
			collector.Add(className, configTxt);
		}
		
		/// <summary>
		/// 添加一个EventIdType
		/// </summary>
		/// <param name="eventName">事件名</param>
		private static void AddEventIdType(string eventName, string scriptNamespace)
		{
			string path = "Assets/Hotfix/Base/Event/EventIdType.cs";
			if (scriptNamespace == "ETModel")
			{
				path = "Assets/Model/Base/Event/EventIdType.cs";
			}
			string text = File.ReadAllText(path);

			// 使用正则匹配到所有EventName
			string pattern = "public const string ([A-Za-z0-9_]+) = \"([A-Za-z0-9_]+)\"";
			MatchCollection matchs = Regex.Matches(text, pattern);

			// 添加一个EventIdType
			using (FileStream fs = new FileStream(path, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
				{
					sw.Write(
$"namespace {scriptNamespace}" +
@"
{
	public static class EventIdType
	{
");
					// 把原来的写上
					for (int i = 0; i < matchs.Count; i++)
					{
						string matchName = matchs[i].Groups[1].Value;
						if (matchName == eventName)
						{
							Debug.LogError($"已经存在事件:{eventName}, 请检查代码.");
							continue;
						}
						sw.WriteLine($"		public const string {matchName} = \"{matchName}\";");
					}
					sw.WriteLine($"		public const string {eventName} = \"{eventName}\";");
					sw.Write(
@"	}
}
");
				}
			}
		}

		/// <summary>
		/// 获取脚本类型
		/// </summary>
		/// <param name="className">类名</param>
		/// <returns>脚本类型</returns>
		private static ScriptType GetScriptType(string className)
		{
			if (className.EndsWith("Component") || className.EndsWith("Entity"))
			{
				return ScriptType.Component;
			}
			else if (className.EndsWith("Event"))
			{
				return ScriptType.Event;
			}
			else if (className.EndsWith("Factory"))
			{
				return ScriptType.Factory;
			}
			else if (className.EndsWith("Config"))
			{
				return ScriptType.Config;
			}
			else
			{
				return ScriptType.Other;
			}
		}

		/// <summary>
		/// 获取生成 New Monobehaviour 中的类名
		/// </summary>
		private static string GetClassName(string text)
		{
			// 判断是否是原生的MonoBehaviour
			if (!text.Contains("// Start is called before the first frame update") && !text.Contains("// Update is called once per frame"))
			{
				return null;
			}

			// 正则匹配
			string pattern = "public class ([A-Za-z0-9_]+)\\s:\\sMonoBehaviour";
			var match = Regex.Match(text, pattern);
			if (match.Success)
			{
				return match.Groups[1].Value;
			}

			return null;
		}
	}
}
