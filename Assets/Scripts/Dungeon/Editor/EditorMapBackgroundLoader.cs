using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;

[CustomEditor(typeof(MapBackgroundLoader))]
public class EditorMapBackgroundLoader : Editor
{
    MapBackgroundLoader script;
    private void OnEnable()
    {
        script = (MapBackgroundLoader)target;
    }
    int selectThemeList = 0, selectBgList = 0;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical("Box");
        {
            script.Theme = 
                (MapTheme) EditorGUILayout.EnumPopup("Theme", script.Theme);

            List<string> bgNames = GetFileNameList(script.GetThemePath());
            selectBgList = EditorGUILayout.Popup("Background", selectBgList, bgNames.ToArray());
            if (bgNames.Count > 0)
                script.BgPath = bgNames[selectBgList];
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Load")) script.LoadMapBackground();
            if (GUILayout.Button("Clear")) script.ClearMapBackground();
        }
        EditorGUILayout.EndHorizontal();
    }
    private List<string> GetFileNameList(string path)
    {
        List<string> ans = new List<string>();
        var arr = Resources.LoadAll(path);
        foreach (var item in arr) ans.Add(item.name);
        return ans;
    }
}
