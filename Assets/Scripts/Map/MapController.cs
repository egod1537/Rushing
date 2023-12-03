using MersenneTwister;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(MapManager))]
public class MapController : MonoBehaviour
{
    MapManager _model;
    MapManager model
    {
        get
        {
            _model = GetComponent<MapManager>();
            return _model;
        }
    }

    MapGenerator generator = new MapGenerator();

    int[] dx = { 1, 0, -1, 0 };
    int[] dy = { 0, 1, 0, -1 };
    char[] dir = { 'D', 'R', 'U', 'L' };

    private void Awake()
    {
        model.OnGenerateMap.AddListener(() => { ResetPlayer(); });
    }

    public void ResetPlayer()
    {
        if (model.player == null) return;
        MoveEntity(model.player, PickStartPos());
    }
    public MapEntity DirectionEntity(Vector2Int pos, int dir)
    {
        Vector2Int nxt = pos + new Vector2Int(dx[dir], dy[dir]);
        if (model.mapEntity.ContainsKey(nxt)) return model.mapEntity[nxt];
        return null;
    }
    public MapEntity DirectionEntity(Vector2Int pos, Direction dir)
        => DirectionEntity(pos, (int)dir);
    public Item DirectionItem(Vector2Int pos, int dir)
    {
        Vector2Int nxt = pos + new Vector2Int(dx[dir], dy[dir]);
        if (model.mapItem.ContainsKey(nxt)) return model.mapItem[nxt];
        return null;
    }
    public Item DirectionItem(Vector2Int pos, Direction dir)
        => DirectionItem(pos, (int)dir);
    public Direction GetDirection(Vector2Int from, Vector2Int to)
    {
        int tx = to.x - from.x, ty = to.y - from.y;
        int nx = (tx == 0 ? 0 : tx / Mathf.Abs(tx)), ny = (ty == 0 ? 0 : ty / Mathf.Abs(ty));
        for (int i = 0; i < 4; i++)
        {
            if (nx == dx[i] && ny == dy[i]) return (Direction)i;
        }
        return Direction.Right;
    }

    public void TeleportEntity(MapEntity entity, Vector2Int pos, UnityAction callback)
    {
        model.brain.AddQueue(() =>
        {
            if (model.mapItem.ContainsKey(pos))
                entity.OnPickup.Invoke(model.mapItem[pos]);

            model.mapEntity.Remove(entity.pos);
            model.mapEntity.Add(pos, entity);

            entity.isMove = true;
            entity.transform.DOKill();
            entity.transform.DOLocalMove(GridToWorldPos(pos), 0.0f)
                .OnComplete(() => {
                    entity.isMove = false;
                    callback();
                })
                .SetEase(Ease.Linear);
            entity.pos = pos;
        });
    }
    public void TeleportEntity(MapEntity entity, Vector2Int pos)
        => TeleportEntity(entity, pos, () =>{});
    public void TeleportEntity(MapEntity entity, int x, int y)
    => TeleportEntity(entity, new Vector2Int(x, y), () => { });

    public void MoveEvent(MapEntity entity, Vector2Int to)
    {
        if (model.mapEntity.ContainsKey(to))
            entity.OnConfilct.Invoke(model.mapEntity[to]);

        if (model.mapItem.ContainsKey(to))
            entity.OnPickup.Invoke(model.mapItem[to]);
    }
    public void MoveEntity(MapEntity entity, Vector2Int pos, UnityAction action)
    {
        if (entity.isMove) return;
        model.brain.AddQueue(() =>
        {
            if (model.mapEntity.ContainsKey(pos))
                entity.OnConfilct.Invoke(model.mapEntity[pos]);
        });
        model.brain.AddQueue(() =>
        {
            if (!CheckMovement(pos)) return;

            if (model.mapItem.ContainsKey(pos))
                entity.OnPickup.Invoke(model.mapItem[pos]);

            if (model.map[pos] == MapObjectType.Portal)
                MapManager.ins.GenerateMap();

            model.mapEntity.Remove(entity.pos);
            model.mapEntity.Add(pos, entity);

            entity.isMove = true;
            entity.OnMove.Invoke(GetDirection(entity.pos, pos));
            entity.transform.DOLocalMove(GridToWorldPos(pos), 1.0f/entity.speed)
                .OnComplete(() => { 
                    entity.isMove = false;
                    action();
                })
                .SetEase(Ease.Linear);
            entity.pos = pos;
        });
    }
    public void MoveEntity(MapEntity entity, Vector2Int pos)
        => MoveEntity(entity, pos, () => { });
    public void MoveEntity(MapEntity entity, int dir, UnityAction action)
    => MoveEntity(entity, entity.pos + new Vector2Int(dx[dir], dy[dir]), action);
    public void MoveEntity(MapEntity entity, int dir)
        => MoveEntity(entity, dir, () => { });
    public void MoveEntity(MapEntity entity, Direction dir)
        => MoveEntity(entity, (int)dir);
    public void RushEntity(MapEntity entity, int dir)
    {
        MoveEntity(entity, dir, () => {
            RushEntity(entity, dir);
        });
    }
    public void RushEntity(MapEntity entity, Direction dir)
        => RushEntity(entity, (int)dir);

    public Vector3 GridToWorldPos(Vector2Int vec)
    => new Vector3(vec.y, -vec.x);
    public List<Vector2Int> GetMapStartPos()
    => generator.FindMapStart(generator.ConvertMap(model.size, model.map));
    public Vector2Int PickStartPos()
    {
        return MapManager.ins.generatorRet.startPos;
        MT19937 mt = new MT19937();
        List<Vector2Int> lt = GetMapStartPos();
        int sz = lt.Count;
        return lt[mt.RandomRange(0, sz - 1)];
    }
    public bool CheckMovement(Vector2Int pos)
    {
        if (!CheckPosition(pos) || !model.map.ContainsKey(pos)) return false;
        return model.map[pos] != MapObjectType.Block && !model.mapEntity.ContainsKey(pos);
    }

    public bool CheckPosition(int x, int y)
    {
        int n = model.size;
        return !(x < 0 || y < 0 || x >= n || y >= n);
    }
    public bool CheckPosition(Vector2Int pos) => CheckPosition(pos.x, pos.y);
}
