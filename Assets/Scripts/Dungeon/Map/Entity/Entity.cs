using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[RequireComponent(typeof(EntityController))]
[RequireComponent(typeof(EntityView))]
public class Entity : MonoBehaviour
{
    public UnityEvent OnCreate = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();

    public UnityEvent<Vector2Int> OnMove = new UnityEvent<Vector2Int>();
    public UnityEvent<Entity, int> OnAttack = new UnityEvent<Entity, int>();
    public UnityEvent<Entity, int> OnDamage = new UnityEvent<Entity, int>();
    public UnityEvent<int> OnHeal = new UnityEvent<int>();

    public MapEntityID id;
    public MapEntityLayer layer;
    public Vector2Int pos;

    public EntityState state;

    public int hp;
    public int maxHp;

    public int offense;
    public float offenseSpeed = 1.0f;

    public int defense;

    public float criticalChance = 0.0f;
    public float criticalDamage = 1.5f;

    public float speed = 1.0f;

    public int Damage(int damage)
    {
        if (state.HasFlag(EntityState.Invincibility)) return 0;

        int ans = damage;
        ans -= defense;
        if (ans < 0) ans = 0;
        return ans;
    }
}
