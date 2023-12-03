using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PlayerModel), true)]
public class EditorPlayerModel : EditorMapEntity
{
    SerializedProperty _energy, maxEnergy;

    PlayerModel script;
    private void OnEnable()
    {
        base.OnEnable();
        script = (PlayerModel)target;

        _energy = serializedObject.FindProperty(nameof(_energy));
        maxEnergy = serializedObject.FindProperty(nameof(maxEnergy));
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.LabelField("Player");
            EditorGUILayout.BeginVertical("HelpBox");
            {
                _energy.intValue = EditorGUILayout.IntField("Energy", _energy.intValue);
                maxEnergy.intValue = EditorGUILayout.IntField("Max Energy", maxEnergy.intValue);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
