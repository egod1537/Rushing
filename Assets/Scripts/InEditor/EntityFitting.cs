using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace InEditor
{
    public class EntityFitting : MonoBehaviour
    {
        [SerializeField]
        private int _MaxEntityInLine;
        public int MaxEntityInLine
        {
            get => _MaxEntityInLine;
            set
            {
                if (_MaxEntityInLine != value)
                {
                    _MaxEntityInLine = value;
                    ReLoad();
                }
            }
        }
        [Serializable]
        private class EntityDictionary : SerializableDictionary<MapEntityID, bool> { }
        [SerializeField]
        private EntityDictionary _activeEntity = new EntityDictionary();
        public void Override()
        {

        }
        public void ReLoad()
        {
            Clear();
            int cnt = 0;
            foreach(var p in _activeEntity)
            {
                if (!p.Value) continue;
                MapEntityID id = p.Key;
                GameObject go = 
                    (GameObject)PrefabUtility.InstantiatePrefab(MapEntityDB.Load(id));

                go.name = $"[{(int)id}]{id}";

                go.transform.SetParent(transform);
                int x = cnt % MaxEntityInLine, y = cnt / MaxEntityInLine;
                go.transform.position = GetPosition(new Vector2Int(x,y));
                cnt++;
            }
        }
        private void Clear()
        {
            int cnt = transform.childCount;
            for (int i = 0; i < cnt; i++) 
                DestroyImmediate(transform.GetChild(0).gameObject);
        }
        public bool GetActive(MapEntityID id)
        {
            if (!_activeEntity.ContainsKey(id))
                _activeEntity[id] = false;
            return _activeEntity[id];        
        }
        public void SetActive(MapEntityID id, bool var)
        {
            if (!_activeEntity.ContainsKey(id))
                _activeEntity[id] = false;
            _activeEntity[id] = var;
        }

        public Vector3 GetPosition(Vector2Int vec)
        {
            return vec + Vector2.one * 0.5f;
        }

        public void SetActiveAll(bool var)
        {
            foreach(MapEntityID id in Enum.GetValues(typeof(MapEntityID)))
                SetActive(id, var);
        }
    }
}
