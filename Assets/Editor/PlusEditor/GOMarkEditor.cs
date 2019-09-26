using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ETModel;
using System.Text;

public class GOMarkEditor : Editor
{
	private const string MenuItemNamae = "GameObject/@刷新 - GameObject";

	[MenuItem(MenuItemNamae, false, 49)]
    public static void Refresh()
	{
		ReferenceCollector collector = Selection.activeGameObject.GetComponent<ReferenceCollector>();
		if (collector)
		{
			collector.Clear();
			GOMark[] marks = collector.GetComponentsInChildren<GOMark>();
			for (int i = 0; i < marks.Length; i++)
			{
				collector.Add(ToVariableName(marks[i].name), marks[i].gameObject);
			}
		}
	}

	private static string ToVariableName(string str)
	{
		StringBuilder retStr = new StringBuilder();
		for (int i = 0; i < str.Length; i++)
		{
			char ch = str[i];
			if (ch >= 'a' && ch <= 'z')
			{
				retStr.Append(ch);
			}
			else if (ch >= 'A' && ch <= 'Z')
			{
				retStr.Append(ch);
			}
			else if (ch >= '0' && ch <= '9')
			{
				retStr.Append(ch);
			}
			else if (ch == '_')
			{
				retStr.Append(ch);
			}
		}
		return retStr.ToString();
	}
}
