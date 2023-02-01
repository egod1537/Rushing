using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MapLayerEntityManager))]
public class EditorMapObjectLayerManager : Editor
{
    MapLayerEntityManager script;
    private void OnEnable()
    {
        script = (MapLayerEntityManager)target;
    }
    Vector2Int position;
    MapEntityID selectedID;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical("Box");
        {
            position = EditorGUILayout.Vector2IntField("Position", position);
            selectedID = (MapEntityID)EditorGUILayout.EnumPopup("ID", selectedID);

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Insert")) 
                    script.Insert(position.x, position.y, selectedID);
                if (GUILayout.Button("Destroy"))
                    script.Destroy(position.x, position.y);
                if (GUILayout.Button("Destroy All"))
                    script.DestroyAll();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}
