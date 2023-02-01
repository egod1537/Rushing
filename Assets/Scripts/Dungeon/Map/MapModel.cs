using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map.MapGenerator;
using System;
using System.Xml.Serialization.Advanced;
using System.ComponentModel;
using System.Text;

[ExecuteInEditMode]
public class MapModel : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _Size;
    public Vector2Int Size
    {
        get => _Size;
        set
        {
            _Size = value;
            Resize();
        }
    }
    public int KeyCount;

    public List<Vector2Int> StartPos = new List<Vector2Int>();
    public List<string> SolveRoute = new List<string>();

    public float _progressGenerateMap;
    public bool _isProgress;

    [Serializable]
    public class LayerDicitionary : SerializableDictionary<MapEntityLayer, MapField> { }
    [SerializeField]
    public LayerDicitionary Layer = new LayerDicitionary();

    [Serializable]
    public class LayerManagerDictionary : SerializableDictionary<MapEntityLayer, MapLayerEntityManager> { }
    [SerializeField]
    public LayerManagerDictionary LayerManager = new LayerManagerDictionary();
    

    public Entity this[MapEntityLayer layer, int x, int y]
    {
        get => Layer[layer][x, y];
        set => Layer[layer][x, y] = value;
    }
    public Entity this[MapEntityLayer layer, Vector2Int pos]
    {
        get => Layer[layer][pos.x, pos.y];
        set => Layer[layer][pos.x, pos.y] = value;
    }

    public Entity AddEntity(MapEntityLayer layer, Vector2Int pos, MapEntityID id)
        => AddEntity(layer, pos.x, pos.y, id);
    public Entity AddEntity(MapEntityLayer layer, int x, int y, MapEntityID id)
    {
        return LayerManager[layer].Insert(x, y, id);
    }

    public bool RemoveEntity(MapEntityLayer layer, int x, int y)
    {
        return LayerManager[layer].Destroy(x, y);
    }

    public void ClearMap()
    {
        _isProgress = false;
        LayerManager = new LayerManagerDictionary();
        foreach (MapEntityLayer e in Enum.GetValues(typeof(MapEntityLayer)))
        {
            Transform tr = transform.GetChild((int)e);
            LayerManager.Add(e,
                tr.GetComponent<MapLayerEntityManager>());
        }

        foreach (MapEntityLayer e in Enum.GetValues(typeof(MapEntityLayer)))
        {
            if (e == MapEntityLayer.Background) continue;
            Transform tr = transform.GetChild((int)e);
            int cnt = tr.childCount;

            for (int i = 0; i < cnt; i++)
                tr.GetChild(i).GetComponent<EntityController>().Death();
        }
        Resize();
        MapBrain.ins.Process();
    }

    private void Resize()
    {
        foreach (MapEntityLayer e in Enum.GetValues(typeof(MapEntityLayer)))
        {
            if (!Layer.ContainsKey(e))
                Layer.Add(e, new MapField(Size.x, Size.y));
            else
                Layer[e].Resize(Size.x, Size.y);
        }
    }
    public void GenerateMap(int iteration = 3000)
    {
        if (_isProgress) return;
        ClearMap();

        MapGenerator generator = new MapGenerator(iteration);
        generator.SetCreator(new MapCreator(new MapValidator()));
        generator.SetFetcher(new MapFetcher());
        generator.SetSpawner(new MapSpawner());

        generator.Generate(this);
        MapBrain.ins.Process();
    }

    public Vector2Int World2TilePos(Vector3 pos)
    {
        return new Vector2Int((int)pos.y, -(int)pos.x);
    }
    public Vector3 Tile2WorldPos(int x, int y)
    {
        return new Vector3(y, -x);
    }
    public Vector3 Tile2WorldPos(Vector2Int pos)
        => Tile2WorldPos(pos.x, pos.y);
    public bool isInside(Vector2Int pos)
        => 0 <= pos.x && pos.x < Size.x && 0 <= pos.y && pos.y < Size.y;
    public bool isPlaceGroundEntity(int x, int y)
    {
        if (this[MapEntityLayer.GroundStructure, x, y] != null)
            return false;
        Entity entity = this[MapEntityLayer.GroundEntity, x, y];
        return entity == null || entity.state.HasFlag(EntityState.Dead);
    }
    public bool isPlaceGroundEntity(Vector2Int pos)
        => isPlaceGroundEntity(pos.x, pos.y);
}
