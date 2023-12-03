using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEntitySummoner : MapSummoner
{
    const string PLAYER_PATH = "Entity/Player";

    Transform _entitys;
    Transform entitys
    {
        get
        {
            if (_entitys == null) _entitys = model.transform.FindChild("entitys");
            return _entitys;
        }
    }

    public void SummonEntity(int x, int y, MapEntity entity)
    => Summon(new Vector2Int(x, y), entity.gameObject);
    public void SummonPlayer()
    {
        if (model.player != null) return; 
        GameObject player = Resources.Load(PLAYER_PATH) as GameObject;
        MapEntity entity = Summon(controller.PickStartPos(), player).GetComponent<MapEntity>();
        model.player = entity;
    }

    public override GameObject Summon(Vector2Int pos, GameObject _go, bool isCreated=false)
    {
        if (!controller.CheckMovement(pos)) return null;
        GameObject go = _go;
        if(!isCreated) go = Object.Instantiate(_go);
        go.name = $"({pos.x}, {pos.y})";

        Transform tr = go.transform;
        tr.SetParent(entitys, false);
        tr.localPosition = controller.GridToWorldPos(pos);

        MapEntity entity = go.GetComponent<MapEntity>();
        entity.pos = pos;
        model.mapEntity.Add(pos, entity);
        entity.OnSummon.Invoke();

        return go;
    }
    public GameObject Summon(Vector2Int pos, MapEntity entity)
        => Summon(pos, entity.gameObject);

    public override void Destroy(Vector2Int pos, GameObject go)
    {
        model.mapEntity.Remove(pos);

        MapEntity entity = go.GetComponent<MapEntity>();
        entity.OnDestroy.Invoke();

        if(entity.entityType != MapEntityType.Player)
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(go);
#else
        Object.Destroy(go);
#endif
        }
    }
    public void Destroy(MapEntity entity)
        => Destroy(entity.pos, entity.gameObject);

    public override void DestroyAll()
    {
        int sz = entitys.childCount;
        List<MapEntity> lt = new List<MapEntity>();
        for (int i = 0; i < sz; i++) lt.Add(entitys.GetChild(i).GetComponent<MapEntity>());
        foreach (var v in lt) Destroy(v.pos, v.gameObject);
    }
}
