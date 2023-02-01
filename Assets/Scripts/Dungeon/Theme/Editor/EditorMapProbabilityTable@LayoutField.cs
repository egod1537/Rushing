using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Map.MapGenerator;
using System;

public partial class EditorMapProbabilityTable : Editor
{
    public class LayoutField : InternalPanel<MapProbabilityTable>
    {
        public LayoutField(MapProbabilityTable script) : base(script)
        {
        }
        bool isFold;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Layout Probability]", EditorStyles.boldLabel);

                foreach (MapLayoutObject e in Enum.GetValues(typeof(MapLayoutObject)))
                {
                    script.layout[e] =
                        EditorGUILayout.Slider(e.ToString(), script.layout[e], 0.0f, 1.0f);
                }
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Clear")) script.layout.Clear();
                    if (GUILayout.Button("Normalize")) script.layout.Normalize();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
