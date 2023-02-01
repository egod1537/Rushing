using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public partial class EditorMapBrain : Editor
{
    public class EntityListPanel : InternalPanel<MapBrain>
    {
        public EntityListPanel(MapBrain script) : base(script)
        {
        }
        bool isFold;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                {
                    isFold = EditorGUILayout.Foldout(isFold, "[Entity List]");
                    EditorGUILayout.IntField(script.entityList.Count, GUILayout.Width(64));
                }
                EditorGUILayout.EndHorizontal();
                if (isFold)
                {
                    foreach(Entity entity in script.entityList)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        {
                            EditorGUILayout.LabelField(
                                $"[{entity.pos.x},{entity.pos.y}] {entity.id}");
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
