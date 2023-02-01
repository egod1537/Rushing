using MersenneTwister;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Map.MapGenerator
{
    [Serializable]
    public class MapCreatorProbabilityTable
    {
        [Serializable]
        private class TableDictionary : SerializableDictionary<MapLayoutObject, float> { }

        private readonly Array MapLayoutObjectArray = Enum.GetValues(typeof(MapLayoutObject));
        [SerializeField]
        private TableDictionary table;
        public MapCreatorProbabilityTable()
        {
            Clear();
        }
        public void Clear()
        {
            table = new TableDictionary();
            foreach (MapLayoutObject e in MapLayoutObjectArray)
                table.Add(e, 0.0f);
        }
        public void Normalize()
        {
            float sum = 0;
            foreach (MapLayoutObject e in MapLayoutObjectArray)
                sum += table[e];
            if (Mathf.Approximately(sum, 0.0f)) return;
            foreach (MapLayoutObject e in MapLayoutObjectArray)
                table[e] /= sum;
        }
        public float this[MapLayoutObject x]
        {
            get => table[x];
            set => table[x] = value;
        }
        public MapLayoutObject get()
        {
            Normalize();
            MT19937 mt = new MT19937();
            float t = mt.RandomRange(0, 1000000) / 1000000.0f;
            float sum = 0;
            foreach (MapLayoutObject e in MapLayoutObjectArray)
            {
                if (sum <= t && t <= sum + table[e])
                    return e;
                sum += table[e];
            }
            return MapLayoutObject.None;
        }
    }
}
