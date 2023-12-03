using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class MapItem : Item
{
    public Vector2Int pos;

    private MapObject _entity;
    public MapObject entity
    {
        get
        {
            if(_entity == null)
                _entity =   GetComponent<MapObject>();
            return _entity;
        }
    }

    private void Awake()
    {
        entity.OnDead.AddListener(() =>
        {
            MapManager.ins.mapItem.Remove(pos);
        });
    }
}
