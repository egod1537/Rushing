using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Entity))]
public class EditorEntity : Editor
{
    Entity script;
    private void OnEnable()
    {
        script = (Entity)target;
    }
    public override void OnInspectorGUI()
    {
        AboutMapPanel();
        AboutAdditionalPanel();
        AboutPropertyPanel();
    }
    public void AboutAdditionalPanel()
    {
        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.LabelField("[Additional]", EditorStyles.boldLabel);
            script.state = (EntityState)EditorGUILayout.EnumFlagsField("State", script.state);
        }
        EditorGUILayout.EndVertical();
    }
    public void AboutPropertyPanel()
    {
        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.LabelField("[Property]", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            {
                script.hp = EditorGUILayout.IntField("Hp", script.hp);
                script.maxHp = EditorGUILayout.IntField("MaxHp", script.maxHp);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            {
                script.offense = EditorGUILayout.IntField("Offense", script.offense);
                script.offenseSpeed = EditorGUILayout.FloatField("Offense Speed (s)", script.offenseSpeed);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            {
                script.defense = EditorGUILayout.IntField("Defense", script.defense);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            {
                script.criticalChance = EditorGUILayout.Slider(
                    "Critical Change", script.criticalChance, 0.0f, 1.0f);
                script.criticalDamage = EditorGUILayout.FloatField("Critical Damage", script.criticalDamage);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            {
                script.criticalChance = EditorGUILayout.FloatField("Speed", script.speed);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }
    public void AboutMapPanel()
    {
        EditorGUILayout.BeginVertical("HelpBox");
        {
            EditorGUILayout.LabelField("[Map]", EditorStyles.boldLabel);
            script.id = (MapEntityID)EditorGUILayout.EnumPopup("ID", script.id);
            script.layer = (MapEntityLayer)EditorGUILayout.EnumPopup("Layer", script.layer);
            script.pos = EditorGUILayout.Vector2IntField("Position", script.pos);
        }
        EditorGUILayout.EndVertical();
    }
}
