using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEntity), true)]
public class EditorMapEntity : Editor 
{
    MapEntity script;

    SerializedProperty 
        entityType, isInvincible,
        _maxHp, plusMaxHp, _hp,
        _attack, plusAttack,
        _attackSpeed, plusAttackSpeed,
        _defense, plusDefense,
        _evasion, plusEvasion,
        _speed, plusSpeed;

    public void OnEnable()
    {
        script = (MapEntity)target;

        entityType = serializedObject.FindProperty(nameof(entityType));
        isInvincible = serializedObject.FindProperty(nameof(isInvincible));

        _maxHp = serializedObject.FindProperty(nameof(_maxHp));
        plusMaxHp = serializedObject.FindProperty(nameof(plusMaxHp));
        _hp = serializedObject.FindProperty(nameof(_hp));

        _attack = serializedObject.FindProperty(nameof(_attack));
        plusAttack = serializedObject.FindProperty(nameof(plusAttack));
        _attackSpeed = serializedObject.FindProperty(nameof(_attackSpeed));
        plusAttackSpeed = serializedObject.FindProperty(nameof(plusAttackSpeed));

        _defense = serializedObject.FindProperty(nameof(_defense));
        plusDefense = serializedObject.FindProperty(nameof(plusDefense));

        _evasion = serializedObject.FindProperty(nameof(_evasion));
        plusEvasion = serializedObject.FindProperty(nameof(plusEvasion));

        _speed = serializedObject.FindProperty(nameof(_speed));
        plusSpeed = serializedObject.FindProperty(nameof(plusSpeed));
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical("HelpBox");
        {
            EditorGUILayout.PropertyField(entityType);
            EditorGUILayout.PropertyField(isInvincible);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                _maxHp.intValue = EditorGUILayout.IntField("MaxHp", _maxHp.intValue);
                plusMaxHp.floatValue = EditorGUILayout.Slider("Plus MaxHp", plusMaxHp.floatValue, 1.0f, 10.0f);
                _hp.intValue = EditorGUILayout.IntField("Hp", _hp.intValue);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            {
                _attack.intValue = EditorGUILayout.IntField("Attack", _attack.intValue);
                plusAttack.floatValue = EditorGUILayout.Slider("Plus Attack", plusAttack.floatValue, 0.0f, 10.0f);
                _attackSpeed.floatValue = EditorGUILayout.Slider("Attack Speed", _attackSpeed.floatValue, 0.0f, 10.0f);
                plusAttackSpeed.floatValue = EditorGUILayout.Slider("Plus Attack Speed", plusAttackSpeed.floatValue, 0.0f, 10.0f);
                EditorGUILayout.LabelField("Time Attack Speed", script.timeAttack.ToString("0.#####"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            {
                _defense.floatValue = EditorGUILayout.Slider("Defense", _defense.floatValue, 0.0f, 1.0f);
                plusDefense.floatValue = EditorGUILayout.Slider("Plus Defense", plusDefense.floatValue, 0.0f, 10.0f);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            {
                _evasion.floatValue = EditorGUILayout.Slider("Evasion", _evasion.floatValue, 0.0f, 1.0f);
                plusEvasion.floatValue = EditorGUILayout.Slider("Plus Evasion", plusEvasion.floatValue, 0.0f, 10.0f);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            {
                _speed.floatValue = EditorGUILayout.FloatField("Speed", _speed.floatValue);
                plusSpeed.floatValue = EditorGUILayout.Slider("Plus Speed", plusSpeed.floatValue, 0.0f, 10.0f);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
