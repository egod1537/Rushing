using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using log4net.Core;

[CustomEditor(typeof(UIInformationBar))]
public class EditorUIInformationBar : Editor
{
    UIInformationBar script;
    private void OnEnable()
    {
        script = (UIInformationBar)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.LabelField("[Reference]", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TxtLevel"));
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ImgHp"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TxtHp"));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ImgEnergy"));
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.LabelField("[Property]", EditorStyles.boldLabel);
            script.Level = EditorGUILayout.IntField("Level", script.Level);
            script.MaxHp = EditorGUILayout.IntField("MaxHp", script.MaxHp);
            script.Hp = EditorGUILayout.IntField("Hp", script.Hp);

            EditorGUILayout.BeginHorizontal();
            {
                script.Energy = EditorGUILayout.IntField("Energy", script.Energy);
                if (GUILayout.Button("-")) script.Energy--;
                if (GUILayout.Button("+")) script.Energy++;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
