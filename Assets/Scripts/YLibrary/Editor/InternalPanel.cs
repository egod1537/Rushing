using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class InternalPanel<T>
{
    protected T script;
    public InternalPanel(T script)
    {
        this.script = script;
    }

    public virtual void OnEnable() { }
    public virtual void OnInspectorGUI() { }
    public virtual void OnSceneGUI() { }
}
