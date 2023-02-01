using Map.MapGenerator;
using MersenneTwister;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapThemeProbabilityTable", 
    menuName ="Scriptable Object/Theme Probability Table",
    order = int.MaxValue)]
[Serializable]
public class MapProbabilityTable : ScriptableObject
{
    [SerializeField]
    public MapCreatorProbabilityTable layout
    = new MapCreatorProbabilityTable();

    [Serializable]
    public class PercentDictionary : SerializableDictionary<MapEntityID, float> { }
    [SerializeField]
    public PercentDictionary dict = new PercentDictionary();

    [Serializable]
    public class DBDictionary : SerializableDictionary<MapEntityLayer, PercentDictionary>
    {
        public new PercentDictionary this[MapEntityLayer layer]
        {
            get
            {
                if(!ContainsKey(layer))
                    Add(layer, new PercentDictionary());
                return base[layer];
            }
            set
            {
                if (!ContainsKey(layer))
                    Add(layer, new PercentDictionary());
                base[layer] = value;
            }
        }
    }
    [SerializeField]
    public DBDictionary db = new DBDictionary();

    public void Normalize()
    {
        foreach(MapEntityLayer layer in Enum.GetValues(typeof(MapEntityLayer)))
        {
            float sum = 0.0f;
            List<MapEntityID> ids = new List<MapEntityID>();

            foreach (var info in db[layer])
            {
                sum += info.Value;
                ids.Add(info.Key);
            }
            if (Mathf.Approximately(sum, 0.0f)) continue;
            foreach (var id in ids)
                db[layer][id] /= sum;
        }
    }

    public MapEntityID Rand(MapEntityLayer layer)
    {
        Normalize();

        MT19937 mt = new MT19937();
        float r = mt.RandomRange(0, 1_000_000) / 1_000_000.0f;
        float sum = 0.0f;
        foreach(var info in db[layer])
        {
            if (sum <= r && r <= sum + info.Value)
                return info.Key;
            sum += info.Value;
        }
        return MapEntityID.Air;
    }

    public float this[MapEntityLayer layer, MapEntityID idx]
    {
        get => db[layer][idx];
        set
        {
            if (db[layer].ContainsKey(idx))
                db[layer].Add(idx, value);
            else db[layer][idx] = value;
        }
    }
}
