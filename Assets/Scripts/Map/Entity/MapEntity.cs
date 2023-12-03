using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class MapEntity : MonoBehaviour
{
    const float DEAD_TIME = 1.0f;

    protected MapManager map { get { return MapManager.ins; } }
    protected MapController mapController { get { return MapManager.ins.controller; } }

    public class SummonEvent : UnityEvent { }
    public SummonEvent OnSummon = new SummonEvent();
    public class DestroyEvent : UnityEvent { }
    public DestroyEvent OnDestroy = new DestroyEvent();

    public MapEntityType entityType;
    public bool isInvincible = false;

    public class PickUpEvent : UnityEvent<MapItem> { }
    public PickUpEvent OnPickup = new PickUpEvent();
    public class ConflictEvent : UnityEvent<MapEntity> { }
    public ConflictEvent OnConfilct = new ConflictEvent();

    public class MoveEvent : UnityEvent<Direction> { }
    public MoveEvent OnMove = new MoveEvent();
    private bool _isMove;
    public bool isMove
    {
        get { return _isMove; }
        set
        {
            if (!value && _isMove) OnStop.Invoke();
            _isMove = value;
        }
    }
    public class StopEvent : UnityEvent { }
    public StopEvent OnStop = new StopEvent();

    public Vector2Int _pos;
    public Vector2Int pos
    {
        get { return _pos; }
        set{_pos = value;}
    }

    public Direction lookDirection = Direction.Left;

    public class EditMaxHpEvent : UnityEvent { }
    public EditMaxHpEvent OnEditMaxHp = new EditMaxHpEvent();
    public int _maxHp;
    public int maxHp
    {
        get { return Mathf.RoundToInt(_maxHp * plusMaxHp); }
        set { 
            _maxHp = value;
            OnEditMaxHp.Invoke();
        }
    }
    public float plusMaxHp = 1.0f;

    public class DamageEvent : UnityEvent<int> { }
    public DamageEvent OnDamage = new DamageEvent();
    public class HealEvent : UnityEvent<int> { }
    public HealEvent OnHeal = new HealEvent();
    public class DeadEvent : UnityEvent { }
    public DeadEvent OnDead = new DeadEvent();
    public class EditHpEvent : UnityEvent { }
    public EditHpEvent OnEditHp = new EditHpEvent();

    public bool isDead;
    public int _hp;
    public int hp
    {
        get { return _hp; }
        set
        {
            _hp = Mathf.Clamp(value, 0, maxHp);
            OnEditHp.Invoke();
        }
    }

    public virtual void Damage(int delta)
    {
        hp -= delta;
        OnDamage.Invoke(delta);

        if (hp == 0)
        {
            isDead = true;
            OnDead.Invoke();
            if(entityType == MapEntityType.Item)
                this.Invoke(() => map.itemSummoner.Destroy(this.GetComponent<MapItem>()), DEAD_TIME);
            else 
                this.Invoke(() => map.entitySummoner.Destroy(this), DEAD_TIME);
        }
    }

    public class AttackEvent : UnityEvent<MapEntity> { }
    public AttackEvent OnAttack = new AttackEvent();
    public int _attack;
    public int attack
    {
        get { return Mathf.CeilToInt(_attack * plusAttack); }
        set { _attack = value; }
    }
    public float plusAttack = 1.0f;

    public float _attackSpeed;
    public float attackSpeed
    {
        get { return _attackSpeed * plusAttackSpeed; }
        set { attackSpeed = value; }
    }
    public float plusAttackSpeed = 1.0f;
    public float timeAttack = 0.0f;

    public void Attack(MapEntity other)
    {
        if (timeAttack > 0.0f && other.entityType != MapEntityType.Item) return;
        timeAttack = 1.0f/attackSpeed;
        other.Damage(attack);

        if (other.entityType != MapEntityType.Item) DamageIndicator.Show(other, attack);
    }

    public class DefenseEvent : UnityEvent<int> { }
    public DefenseEvent OnDefense = new DefenseEvent();
    public float _defense;
    public float defense
    {
        get { return _defense * plusDefense; }
        set { _defense = value; }
    }
    public float plusDefense = 1.0f;

    public class EvasionEvent : UnityEvent<int> { }
    public EvasionEvent OnEvasion = new EvasionEvent();
    public float _evasion;
    public float evasion
    {
        get { return _evasion * plusEvasion; }
        set { _evasion = value; }
    }
    public float plusEvasion = 1.0f;

    public class SpeedEvent : UnityEvent<int> { }
    public SpeedEvent OnSpeed = new SpeedEvent();
    public float _speed;
    public float speed
    {
        get { return _speed*plusSpeed; }
        set { _speed = value; }
    }
    public float plusSpeed = 1.0f;

    public virtual void Update()
    {
        if(timeAttack > 0.0f)
        {
            timeAttack -= Time.deltaTime;
        }
    }
}
