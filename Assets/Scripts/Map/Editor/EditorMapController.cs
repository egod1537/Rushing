using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MapController))]
public class EditorMapController : Editor
{
    MapController script;
    private void OnEnable()
    {
        script = (MapController)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Reset Player")) script.ResetPlayer();
    }
}
