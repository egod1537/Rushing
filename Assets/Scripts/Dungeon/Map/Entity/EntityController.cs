using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
[RequireComponent(typeof(Entity))]
public class EntityController : MonoBehaviour
{
    private Entity _entity;
    public Entity entity { get=>_entity??=GetComponent<Entity>(); }

    MapModel map { get => MapBrain.ins.model; }

    public virtual void ReceiveMove(Vector2Int to)
    {
        Vector2Int pos = entity.pos;

        entity.pos = to;
        entity.transform.DOLocalMove(map.Tile2WorldPos(to), 0.25f);
        entity.OnMove.Invoke(to - pos);
    }
    public virtual void ReceiveAttack(Entity target)
    {
        int damage = entity.offense;
        if (rand() <= entity.criticalChance)
            damage = Mathf.RoundToInt(damage * entity.criticalDamage);

        entity.OnAttack.Invoke(entity, target.Damage(damage));
    }
    public virtual void ReceiveSkill(Entity target, ISkill skill)
    {
        
    }
    public virtual void ReceiveDeath()
    {
        entity.OnDeath.Invoke();
    }

    public void Move(Vector2Int pos, UnityAction<bool> callback=null)
    {
        MapBrain.ins.Move(this, pos, callback);
    }
    public void Attack(Vector2Int pos, UnityAction<bool> callback = null)
    {
        MapBrain.ins.Attack(this, pos, callback);
    }
    public void Skill(Vector2Int pos, ISkill skill, UnityAction<bool> callback = null)
    {
        MapBrain.ins.Skill(this, pos, skill, callback);
    }
    public void Death(UnityAction<bool> callback = null)
    {
        MapBrain.ins.Death(this, callback);
    }

    public bool isEntity(Direction dir)
    {
        int d = (int)dir;
        Vector2Int pos = entity.pos + new Vector2Int(MapTool.dx[d], MapTool.dy[d]);
        if (!map.isInside(pos)) return false;
        return map.isPlaceGroundEntity(pos);
    }

    public double rand()
        => new Unity.Mathematics.Random().NextDouble(0.0, 1.0);
}
