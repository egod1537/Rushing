using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ThemeObject
{
    [SerializeField]
    public GameObject go;
    [SerializeField]
    public Vector2Int size;
    [SerializeField]
    public float percent;
}
[Serializable]
public class ThemeObjectArray
{
    [SerializeField]
    public ThemeObject[] array;
}
[CreateAssetMenu(fileName = "SOMapTheme", menuName = "Rushing/Map Theme",  order = int.MaxValue)]
public class SOMapTheme : ScriptableObject
{
    public GameObject[] bg;

    [Serializable]
    public class TileDictionary : SerializeDictionary<MapObjectType, ThemeObjectArray> { };
    [SerializeField]
    public TileDictionary tiles = new TileDictionary();

    public ThemeObject rand(MapObjectType map)
    {
        if (map == MapObjectType.None) 
            return new ThemeObject();
        return rand(tiles[map].array);
    }
    public ThemeObject rand(ThemeObject[] theme)
    {
        float t = UnityEngine.Random.Range(0.0f, 1.0f);
        float sum = 0.0f;
        foreach(var o in theme)
        {
            if (sum <= t && t <= sum + o.percent)
                return o;
            sum += o.percent;
        }
        return new ThemeObject();
    }

    public void NormalizePercent(MapObjectType target)
    {
        float sum = 0.0f;
        foreach (var t in tiles[target].array) sum += t.percent;
        if (Mathf.Approximately(sum, 0.0f)) return;
        int sz = tiles[target].array.Length;
        for (int i = 0; i < sz; i++) tiles[target].array[i].percent /= sum;
    }
}
