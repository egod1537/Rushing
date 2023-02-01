using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(MapProbabilityTable))]
public partial class EditorMapProbabilityTable : Editor
{
    MapProbabilityTable script;
    List<InternalPanel<MapProbabilityTable>> panels;
    private void OnEnable()
    {
        script = (MapProbabilityTable)target;

        panels = new List<InternalPanel<MapProbabilityTable>>();
        panels.Add(new LayoutField(script));
        panels.Add(new ObjectPanel(script));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        foreach(var panel in panels) panel.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(script);
    }
}
