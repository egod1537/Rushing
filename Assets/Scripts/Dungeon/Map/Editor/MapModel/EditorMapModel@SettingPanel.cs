using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public partial class EditorMapModel : Editor
{
    public class SettingPanel : InternalPanel<MapModel>
    {
        public SettingPanel(MapModel script) : base(script)
        {
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Setting]", EditorStyles.boldLabel);
                script.Size = EditorGUILayout.Vector2IntField("Size", script.Size);
                script.KeyCount = EditorGUILayout.IntField("Key Count", script.KeyCount);
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
    }
}
