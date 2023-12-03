using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Reflection;

[CustomEditor(typeof(SOMapTheme))]
public class EditorSOMapTheme : Editor
{
    SOMapTheme script;
    Dictionary<MapObjectType, ReorderableList> reorderableList;
    private void OnEnable()
    {
        script = (SOMapTheme)target;
        reorderableList = new Dictionary<MapObjectType, ReorderableList>();
        int idx = 0;
        foreach (MapObjectType mo in Enum.GetValues(typeof(MapObjectType)))
        {
            if (mo == MapObjectType.None) continue;
            if (!script.tiles.ContainsKey(mo))
            {
                serializedObject.Update();
                script.tiles.Add(mo, new ThemeObjectArray());
                serializedObject.ApplyModifiedProperties();
            }

            serializedObject.Update();
            var prop = serializedObject.FindProperty(
                $"tiles.values.Array.data[{idx++}].array");

            reorderableList.Add(mo, new ReorderableList(serializedObject, prop));

            reorderableList[mo].elementHeight = 70;
            reorderableList[mo].drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = prop.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element);
            };
            reorderableList[mo].drawHeaderCallback = (rect) =>
            {
                Rect btnRect = new Rect(rect)
                {
                    width = 96
                };
                if (GUI.Button(btnRect, "Normalize")) script.NormalizePercent(mo);
                Rect labelRect = new Rect(btnRect)
                {
                    x = btnRect.x + 96 + 2
                };
                EditorGUI.LabelField(labelRect, $"{mo.ToString()} Tiles");
            };
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bg"));
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.LabelField("Theme Tiles");
            foreach (MapObjectType mo in Enum.GetValues(typeof(MapObjectType)))
            {
                if (mo == MapObjectType.None) continue;
                serializedObject.Update();
                reorderableList[mo].DoLayoutList();
                serializedObject.ApplyModifiedProperties();
            }
        }
        EditorGUILayout.EndVertical();

        Undo.RecordObject(script, "Record SO Map Theme");
        serializedObject.ApplyModifiedProperties();
    }
}
