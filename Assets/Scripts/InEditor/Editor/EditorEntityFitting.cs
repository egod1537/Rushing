using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using PlasticPipe.PlasticProtocol.Messages;

namespace InEditor
{
    [CustomEditor(typeof(EntityFitting))]
    public class EditorEntityFitting : Editor
    {
        EntityFitting script;
        private void OnEnable()
        {
            script = (EntityFitting)target;
        }
        public override void OnInspectorGUI()
        {
            OptionPanel();
            ActivePanel();
        }

        public void OptionPanel()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUI.indentLevel++;
                script.MaxEntityInLine = 
                    EditorGUILayout.IntField("Max In Line", script.MaxEntityInLine);

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Overriding")) script.Override();
                    if (GUILayout.Button("ReLoad")) script.ReLoad();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        bool isFoldActivePanel;
        public void ActivePanel()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;
                isFoldActivePanel = 
                    EditorGUILayout.Foldout(isFoldActivePanel, "[Active Panel]");
                
                if (isFoldActivePanel)
                {
                    foreach(MapEntityID id in Enum.GetValues(typeof(MapEntityID)))
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            script.SetActive(id,
                                EditorGUILayout.Toggle(script.GetActive(id), GUILayout.Width(64)));

                            GUIStyleState labelStyleState = new GUIStyleState()
                            {
                                textColor = (script.GetActive(id) ? Color.white : Color.gray)
                            };
                            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
                            {
                                normal = labelStyleState
                            };
                            EditorGUILayout.LabelField($"{id}", labelStyle);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }
    }
}