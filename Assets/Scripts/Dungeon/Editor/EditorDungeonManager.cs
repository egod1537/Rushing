using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DungeonManager))]
public class EditorDungeonManager : Editor
{
    DungeonManager script;
    private void OnEnable()
    {
        script = (DungeonManager) target;
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical("Box");
        {
            script.Theme = (MapTheme)EditorGUILayout.EnumPopup("Theme", script.Theme);
            script.Floor = EditorGUILayout.IntField("Floor", script.Floor);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.ObjectField("Table", script.table, typeof(MapProbabilityTable));
    }
}
