using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(MapModel))]
public partial class EditorMapModel : Editor
{
    MapModel script;
    List<InternalPanel<MapModel>> panels;
    private void OnEnable()
    {
        script = (MapModel)target;
        panels = new List<InternalPanel<MapModel>>();

        panels.Add(new SettingPanel(script));
        panels.Add(new DebuggerPanel(script));
        panels.Add(new GeneratePanel(script));
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        foreach (var panel in panels) panel.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
    }
}
