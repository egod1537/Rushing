using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public partial class EditorMapModel : Editor
{
    public class DebuggerPanel : InternalPanel<MapModel>
    {
        public DebuggerPanel(MapModel script) : base(script)
        {
        }
        bool isFold;

        MapEntityLayer selectLayer;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isFold = EditorGUILayout.Foldout(isFold, "[Debugger]");
                if (isFold)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        selectLayer = (MapEntityLayer)EditorGUILayout.EnumPopup(
                            "Layer", selectLayer);
                        if (GUILayout.Button("View"))
                            Debug.Log(script.Layer[selectLayer].ToString());
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
