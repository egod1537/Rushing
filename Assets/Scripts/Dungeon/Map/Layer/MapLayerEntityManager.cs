using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapLayerEntityManager : MonoBehaviour
{
    public MapEntityLayer Layer;

    MapModel _model;
    MapModel model
    {
        get
        {
            if(_model == null)
            {
                Transform now = transform;
                while(now.parent != null && _model == null)
                {
                    now = now.parent;
                    _model = now.GetComponent<MapModel>();
                }
            }
            return _model;
        }
    }
    MapField field { get => model.Layer[Layer]; }

    public Entity Insert(int x, int y, MapEntityID id)
    {
        if (id == MapEntityID.Air) return null;
        if (field[x, y] != null) return null;

        GameObject go = Instantiate(MapEntityDB.Load(id));
        go.transform.SetParent(transform);
        go.transform.localPosition = model.Tile2WorldPos(x, y);

        field[x, y] = go.GetComponent<Entity>();

        return field[x,y];
    }
    public bool Insert(Vector2Int pos, MapEntityID id)
        => Insert(pos.x, pos.y, id);

    public bool Destroy(int x, int y)
    {
        if (field[x,y] == null) return false;
        DestroyImmediate(field[x,y].gameObject);
        field[x, y] = null;

        return true;
    }
    public bool Destroy(Vector2Int pos) => Destroy(pos.x, pos.y);
    public int Destroy(MapEntityID id)
    {
        Vector2Int sz = model.Size;
        for (int i = 0; i < sz.x; i++)
            for (int j = 0; j < sz.y; j++)
                if (field[i,j] != null && field[i, j].id == id)
                {
                    Destroy(i, j);
                    return 1;
                }
        return 0;
    }
    public int DestroyAll(MapEntityID id)
    {
        int ans = 0;
        while(Destroy(id) > 0) ans++;
        return ans;
    }
    public int DestroyAll()
    {
        Vector2Int sz = model.Size;
        for (int i = 0; i < sz.x; i++)
            for (int j = 0; j < sz.y; j++)
                Destroy(i, j);
        return 0;
    }
}
