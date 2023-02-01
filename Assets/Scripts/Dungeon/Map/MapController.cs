using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

[RequireComponent(typeof(MapModel))]
public class MapController : MonoBehaviour
{
    private MapModel _model;
    private MapModel model { get =>_model ??= GetComponent<MapModel>(); }

    public bool Move(EntityController controller, Vector2Int to)
    {
        Entity entity = controller.entity;

        if (entity.pos == to) return false;
        if (!model.isInside(to)) return false;
        if (CheckEntity(entity.layer, to))
            return false;
        if (entity.layer == MapEntityLayer.GroundEntity && !model.isPlaceGroundEntity(to))
            return false;
        model[entity.layer, to] = entity;
        model[entity.layer, entity.pos] = null;

        controller.ReceiveMove(to);

        return true;
    }
    
    public bool Attack(EntityController controller, Vector2Int to)
    {
        Entity entity = controller.entity;
        if (!model.isInside(to)) return false;
        if (!CheckEntity(entity.layer, to)) 
            return false;
        controller.ReceiveAttack(model[entity.layer, to]);

        return true;
    }
    public bool Skill(EntityController controller, Vector2Int to, ISkill skill)
    {
        Entity entity = controller.entity;
        if (!model.isInside(to)) return false;
        if (!CheckEntity(entity.layer, to))
            return false;
        controller.ReceiveSkill(model[entity.layer, to], skill);

        return true;
    }
    public bool Create(MapEntityLayer layer, Vector2Int to, MapEntityID id)
    {
        if(!model.isInside(to)) return false;
        if (CheckEntity(layer, to)) return false;
        if (layer == MapEntityLayer.GroundEntity && !model.isPlaceGroundEntity(to))
            return false;

        Entity entity = model.AddEntity(layer, to, id);
        entity.layer = layer;
        entity.pos = to;
        entity.id = id;

        entity.transform.localPosition = model.Tile2WorldPos(to);

        MapBrain.ins.entityList.Add(entity);

        return true;
    }
    public bool Death(EntityController controller)
    {
        Entity entity = controller.entity;
        controller.ReceiveDeath();
        model.RemoveEntity(entity.layer, entity.pos.x, entity.pos.y);

        return true;
    }

    private bool CheckEntity(MapEntityLayer layer, int x, int y)
        => model[layer, x, y] != null && !model[layer, x, y].state.HasFlag(EntityState.Dead);
    private bool CheckEntity(MapEntityLayer layer, Vector2Int pos)
        => CheckEntity(layer, pos.x, pos.y);
}
