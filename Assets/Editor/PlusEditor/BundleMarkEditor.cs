using UnityEditor;
using System.IO;
using UnityEngine;

namespace ETPlus
{
	[InitializeOnLoad]
	public class ResKitAssetsMenu
	{
		public const string AssetBundlesOutputPath = "AssetBundles";
		private static int mSimulateAssetBundleInEditor = -1;
		private const string kSimulateAssetBundles = "SimulateAssetBundles";

		// Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
		public static bool SimulateAssetBundleInEditor
		{
			get
			{
				if (mSimulateAssetBundleInEditor == -1)
					mSimulateAssetBundleInEditor = EditorPrefs.GetBool(kSimulateAssetBundles, true) ? 1 : 0;

				return mSimulateAssetBundleInEditor != 0;
			}
			set
			{
				var newValue = value ? 1 : 0;
				if (newValue != mSimulateAssetBundleInEditor)
				{
					mSimulateAssetBundleInEditor = newValue;
					EditorPrefs.SetBool(kSimulateAssetBundles, value);
				}
			}
		}

		private const string Mark_AssetBundle = "Assets/@标记 - AssetBundle";

		static ResKitAssetsMenu()
		{
			Selection.selectionChanged = OnSelectionChanged;
		}

		public static void OnSelectionChanged()
		{
			var path = GetSelectedPathOrFallback();
			if (!string.IsNullOrEmpty(path))
			{
				Menu.SetChecked(Mark_AssetBundle, Marked(path));
			}
		}

		public static bool Marked(string path)
		{
			var ai = AssetImporter.GetAtPath(path);
			var dir = new DirectoryInfo(path);
			return string.Equals(ai.assetBundleName, $"{dir.Name.Split('.')[0]}.unity3d".ToLower());
		}

		public static void MarkPTAB(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				var ai = AssetImporter.GetAtPath(path);
				var dir = new DirectoryInfo(path);

				if (Marked(path))
				{
					Menu.SetChecked(Mark_AssetBundle, false);
					ai.assetBundleName = null;
				}
				else
				{
					Menu.SetChecked(Mark_AssetBundle, true);
					ai.assetBundleName = $"{dir.Name.Split('.')[0]}.unity3d".ToLower();
				}

				AssetDatabase.RemoveUnusedAssetBundleNames();
			}
		}


		[MenuItem(Mark_AssetBundle, false, 10000)]
		public static void MarkPTABDir()
		{
			var path = GetSelectedPathOrFallback();
			MarkPTAB(path);
		}

		[MenuItem(Mark_AssetBundle, true, 10000)]
		public static bool MarkPTABDir_Check()
		{
			string path = GetSelectedPathOrFallback();
			if (string.IsNullOrEmpty(path) == false)
			{
				if (path.Split('.').Length == 1)
				{
					return false;
				}
				else if (path.EndsWith(".cs"))
				{
					return false;
				}
			}

			return true;
		}

		public static string GetSelectedPathOrFallback()
		{
			var path = string.Empty;

			foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if (!string.IsNullOrEmpty(path) && File.Exists(path))
				{
				}
			}

			//Debug.Log ("path ***** :"+path);
			return path;
		}
	}
}