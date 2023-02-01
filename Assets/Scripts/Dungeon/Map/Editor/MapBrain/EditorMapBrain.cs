using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static EntityMapBrain;

[CustomEditor(typeof(MapBrain))]
public partial class EditorMapBrain : Editor
{
    MapBrain script;
    List<InternalPanel<MapBrain>> panels;
    private void OnEnable()
    {
        script = (MapBrain)target;

        panels = new List<InternalPanel<MapBrain>>();

        panels.Add(new EntityListPanel(script));
        panels.Add(new QueueListPanel(script));
    }

    public override void OnInspectorGUI()
    {
        foreach (var panel in panels) panel.OnInspectorGUI();
        if (GUILayout.Button("Process")) script.Process();
    }
}
