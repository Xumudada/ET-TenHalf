using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using ETModel;
using System.IO;

namespace ETPlus
{
	public class AlwaysLoadSceneWindowData
	{
		public string initSceneName = "Init";
		public bool isEnabled = true;
	}

	public class AlwaysLoadSceneWindow : EditorWindow
	{
		private const string path = @"./Assets/Res/Config/AlwaysLoadSceneWindowData.txt";
		private AlwaysLoadSceneWindowData data;

		[MenuItem("Tools/Plus/Start Scene", priority = 0)]
		private static void ShowWindow()
		{
			AlwaysLoadSceneWindow window = EditorWindow.GetWindow<AlwaysLoadSceneWindow>() as AlwaysLoadSceneWindow;
			window.minSize = new Vector2(300, 50);
			window.Show();
		}

		private void OnEnable()
		{
			this.titleContent = new GUIContent("Start Scene");
			if (File.Exists(path))
			{
				this.data = JsonHelper.FromJson<AlwaysLoadSceneWindowData>(File.ReadAllText(path));
			}
			else
			{
				this.data = new AlwaysLoadSceneWindowData();
				Save();
			}
		}

		private void OnGUI()
		{
			GUILayout.BeginVertical("box", GUILayout.Width(300));
			{
				GUILayout.Space(5);
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label("起始场景设置");
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5);

				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("初始场景:");
					string currentName = GUILayout.TextField(data.initSceneName, GUILayout.Width(200));
					if (currentName != data.initSceneName)
					{
						data.initSceneName = currentName;
						Save();
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("是否启用:");
					bool currentEnabled = EditorGUILayout.Toggle(data.isEnabled);
					if (currentEnabled != data.isEnabled)
					{
						data.isEnabled = currentEnabled;
						Save();
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}

		private void Save()
		{
			File.WriteAllText(path, JsonHelper.ToJson(this.data));
			AssetDatabase.Refresh();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void OnGameEnter()
		{
			if (File.Exists(path))
			{
				AlwaysLoadSceneWindowData data = JsonHelper.FromJson<AlwaysLoadSceneWindowData>(File.ReadAllText(path));
				if (data.isEnabled == true && string.IsNullOrEmpty(data.initSceneName) == false)
				{
					if (SceneManager.GetActiveScene().name != data.initSceneName)
					{
						SceneManager.LoadScene(data.initSceneName);
					}
				}
			}
		}
	}
}
