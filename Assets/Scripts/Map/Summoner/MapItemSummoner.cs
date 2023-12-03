using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemSummoner : MapSummoner
{
    Transform _items;
    Transform items
    {
        get
        {
            if (_items == null) _items = model.transform.FindChild("items");
            return _items;
        }
    }

    public override void Destroy(Vector2Int pos, GameObject go)
    {
        if(model.mapItem.ContainsKey(pos)) model.mapItem.Remove(pos);
#if UNITY_EDITOR
        Object.DestroyImmediate(go);
#else
        Object.Destroy(go);
#endif
    }
    public void Destroy(MapItem item)
        => Destroy(item.pos, item.gameObject);

    public override void DestroyAll()
    {
        int sz = items.childCount;
        List<MapItem> lt = new List<MapItem>();
        for (int i = 0; i < sz; i++) lt.Add(items.GetChild(i).GetComponent<MapItem>());
        foreach (var v in lt) Destroy(v.pos, v.gameObject);
    }

    public override GameObject Summon(Vector2Int pos, GameObject _go, bool isCreated=false)
    {
        if (model.mapItem.ContainsKey(pos)) return null;
        GameObject go = _go;
        if (!isCreated) go = Object.Instantiate(_go);

        Transform tr = go.transform;
        tr.SetParent(items, false);
        tr.localPosition = controller.GridToWorldPos(pos);

        MapItem item = go.GetComponent<MapItem>();
        item.pos = pos;

        MapEntity entity = go.GetComponent<MapEntity>();
        entity.pos = pos;

        model.mapItem.Add(pos, item);
        return go;
    }
}
