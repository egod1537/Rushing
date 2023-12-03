using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapObjectSummoner : MapSummoner
{
    Transform _objects;
    Transform objects
    {
        get
        {
            if (_objects == null) _objects = model.transform.FindChild("objects");
            return _objects;
        }
    }

    public override void Destroy(Vector2Int pos, GameObject go)
    {
        model.mapObjects.Remove(pos);
        model.map[pos] = MapObjectType.None;

#if UNITY_EDITOR
        Object.DestroyImmediate(go);
#else
        Object.Destroy(go);
#endif
    }

    public override void DestroyAll()
    {
        int sz = objects.childCount;
        List<MapObject> lt = new List<MapObject>();
        for(int i=0; i < sz; i++)
            lt.Add(objects.GetChild(i).GetComponent<MapObject>());
        foreach (var v in lt) Destroy(v.pos, v.gameObject);
    }

    public override GameObject Summon(Vector2Int pos, GameObject _go, bool isCreated=false)
    {
        if (model.mapObjects.ContainsKey(pos)) return null;
        GameObject go = _go;
        if(!isCreated) go = Object.Instantiate(_go);
        go.name = $"({pos.x}, {pos.y})";

        Transform tr = go.transform;
        tr.SetParent(objects, false);
        tr.localPosition = controller.GridToWorldPos(pos);

        model.mapObjects.Add(pos, go);

        MapObject obj = go.GetComponent<MapObject>();
        obj.pos = pos;
        obj.objectType = model.map[pos];

#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(go, $"Create Tile ({pos.x}, {pos.y})");
#endif
        return go;
    }
}
