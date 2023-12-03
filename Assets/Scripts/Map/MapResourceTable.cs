using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapResourceTable
{
    [Serializable]
    public class PercentDictionary : SerializeDictionary<MapObjectType, float> { }

    [SerializeField] public int n, treasureCount, monsterCount;
    [SerializeField] public MapType mapType;
    [SerializeField] public PercentDictionary percent;

    public MapResourceTable()
    {
        percent = new PercentDictionary();
        foreach (MapObjectType e in Enum.GetValues(typeof(MapObjectType)))
            percent.Add(e, 0.0f);
    }
    public MapResourceTable(int _n, MapType type) : this()
    {
        n = _n;
        mapType = type;
    }
    public MapResourceTable SetNone(float t)
    {
        percent[MapObjectType.None] = t;
        return this;
    }
    public MapResourceTable SetDamagable(float t)
    {
        percent[MapObjectType.Damagable] = t;
        return this;
    }
    public MapResourceTable SetBlock(float t)
    {
        percent[MapObjectType.Block] = t;
        return this;
    }
    public MapResourceTable SetTreasure(float t)
    {
        percent[MapObjectType.Treasure] = t;
        return this;
    }
    public MapResourceTable SetTreasureCount(int count)
    {
        treasureCount = count;
        return this;
    }
    public void Normalize()
    {
        float sum = 0.0f;
        foreach (var p in percent) sum += p.Value;
        if (Mathf.Approximately(sum, 0.0f)) return;
        foreach (MapObjectType e in Enum.GetValues(typeof(MapObjectType)))
            percent[e] /= sum;
    }
    public MapObjectType rand(System.Random r)
        => rand(r.NextDouble());
    public MapObjectType rand(double t)
    {
        float sum = 0.0f;
        foreach (MapObjectType e in Enum.GetValues(typeof(MapObjectType)))
        {
            if (sum <= t && t <= sum + percent[e])
                return e;
            sum += percent[e];
        }
        return MapObjectType.None;
    }
}
