using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using ETEditor;
using Debug = UnityEngine.Debug;
using System.IO;
using System;
using ETModel;

namespace ETPlus
{
	public class ScpWindowData
	{
		public string serverIP = "119.23.241.65";
		public string username = "root";
		public string serverProgramPath = "/root/ET";
		public string serverBundlePath = "/var/www/html/ET";
		public PlatformType platformType = PlatformType.None;
	}

	public class ScpWindow : EditorWindow
	{
		private const string path = @"./Assets/Res/Config/ScpWindowData.txt";
		private ScpWindowData scpWindowData;

		[MenuItem("Tools/Plus/Scp Window #s", priority = 1)]
		private static void ShowWindow()
		{
			ScpWindow scpWindow = EditorWindow.GetWindow<ScpWindow>() as ScpWindow;
			scpWindow.minSize = new Vector2(400, 250);
			scpWindow.Show();
		}

		private void OnEnable()
		{
			this.titleContent = new GUIContent("Scp Window");
			if (File.Exists(path))
			{
				this.scpWindowData = JsonHelper.FromJson<ScpWindowData>(File.ReadAllText(path));
			}
			else
			{
				this.scpWindowData = new ScpWindowData();
			}
		}

		private void OnGUI()
		{
			// 内容
			GUILayout.BeginVertical("box", GUILayout.Width(400), GUILayout.Height(250));
			{
				GUILayout.Space(5);
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label("SCP同步工具");
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5);

				// IP地址输入框
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("服务器ip:");
					string currentIP = GUILayout.TextField(scpWindowData.serverIP, GUILayout.Width(250));
					if (currentIP != scpWindowData.serverIP)
					{
						scpWindowData.serverIP = currentIP;
						Save();
					}
				}
				GUILayout.EndHorizontal();

				// 用户名输入框
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("用户名:");
					string currentUsername = GUILayout.TextField(scpWindowData.username, GUILayout.Width(250));
					if (scpWindowData.username != currentUsername)
					{
						scpWindowData.username = currentUsername;
						Save();
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.Space(20);

				// 同步热更程序
				GUILayout.BeginVertical("box", GUILayout.Width(400), GUILayout.Height(50));
				{
					// 服务器程序地址
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label("服务器程序地址:");
						string currentProgramPath = GUILayout.TextField(scpWindowData.serverProgramPath, GUILayout.Width(250));
						if (scpWindowData.serverProgramPath != currentProgramPath)
						{
							scpWindowData.serverProgramPath = currentProgramPath;
							Save();
						}
					}
					GUILayout.EndHorizontal();

					GUILayout.Space(20);

					if (GUILayout.Button("同步程序"))
					{
						Debug.Log("同步程序");
						string localProgramPath = Application.dataPath.Replace("Unity/Assets", $"Bin/publish");
						if (Directory.Exists(localProgramPath) == false)
						{
							Debug.LogError($"不存在路径: {localProgramPath}, 请检查是否 dotnet push ?");
							return;
						}

						string arguments = $"-r {localProgramPath} {scpWindowData.username}@{scpWindowData.serverIP}:{scpWindowData.serverProgramPath}";

						Debug.Log($"同步服务器程序, 命令: scp {arguments}");
						ProcessHelper.Run("scp", arguments);
					}
				}
				GUILayout.EndVertical();

				GUILayout.Space(20);

				// 同步热更资源
				GUILayout.BeginVertical("box", GUILayout.Width(400), GUILayout.Height(50));
				{
					// 服务器资源地址
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label("服务器资源地址:");
						string currentBundlePath = GUILayout.TextField(scpWindowData.serverBundlePath, GUILayout.Width(250));
						if (scpWindowData.serverBundlePath != currentBundlePath)
						{
							scpWindowData.serverBundlePath = currentBundlePath;
							Save();
						}
					}
					GUILayout.EndHorizontal();

					GUILayout.Space(10);

					// 平台类型
					PlatformType currentPlatform = (PlatformType)EditorGUILayout.EnumPopup(scpWindowData.platformType);
					if (scpWindowData.platformType != currentPlatform)
					{
						scpWindowData.platformType = currentPlatform;
						Save();
					}

					GUILayout.Space(10);

					if (GUILayout.Button("同步资源"))
					{
						if (scpWindowData.platformType == PlatformType.None)
						{
							Debug.LogError("请选择平台, 当前为: None");
						}
						else
						{
							// Release下的热更目录
							string platformName = Enum.GetName(typeof(PlatformType), scpWindowData.platformType);
							string localBundlePath = Application.dataPath.Replace("Unity/Assets", $"Release/{platformName}");
							if (Directory.Exists(localBundlePath) == false)
							{
								Debug.LogError($"不存在路径: {localBundlePath}, 请检查是否打包此平台的AssetBundle"); 
								return;
							}

							string arguments = $"-r {localBundlePath} {scpWindowData.username}@{scpWindowData.serverIP}:{scpWindowData.serverBundlePath}";

							Debug.Log($"同步服务器资源, 命令: scp {arguments}");
							ProcessHelper.Run("scp", arguments);
						}
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
		}

		private void Save()
		{
			File.WriteAllText(path, JsonHelper.ToJson(this.scpWindowData));
			AssetDatabase.Refresh();
		}
	}
}