using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModel : MapEntity
{
    public const float INVINCIBLE_TIME = 1.0f;
    public class StartInvincibleEvent : UnityEvent { }
    public StartInvincibleEvent OnStartInvincible = new StartInvincibleEvent();
    public class StayInvincibleEvent : UnityEvent<float> { }
    public StayInvincibleEvent OnStayInvincible = new StayInvincibleEvent();
    public class EndInvincibleEvent : UnityEvent { }
    public EndInvincibleEvent OnEndInvincible = new EndInvincibleEvent();

    public float timeInvincible = 0.0f;

    public class EnergyEvent : UnityEvent { }
    public EnergyEvent OnEditEnergy = new EnergyEvent();

    public int level;

    public int _energy;
    public int energy
    {
        get { return _energy; }
        set
        {
            _energy = Mathf.Clamp(value, 0, maxEnergy);
            OnEditEnergy.Invoke();
        }
    }
    public int maxEnergy;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();

        OnPickup.AddListener((MapItem item) =>
        {
            Attack(item.entity);
            if (item.code == ItemCode.Treasure) MapManager.ins.treasure--;
        });

        OnConfilct.AddListener((MapEntity other) =>
        {
            if(other.entityType == MapEntityType.Monster)
                Attack(other);
        });

        OnMove.AddListener((Direction dir) => {
            int dx = 0, dy = 0;
            if (dir == Direction.Left) dx = -1;
            if (dir == Direction.Right) dx = 1;
            if (dir == Direction.Up) dy = 1;
            if (dir == Direction.Down) dy = -1;

            animator.SetFloat("MoveX", dx);
            animator.SetFloat("MoveY", dy);
            animator.SetTrigger("Rush");
        });
    }

    public override void Update()
    {
        base.Update();
        if (timeInvincible > 0.0f)
        {
            OnStayInvincible.Invoke(timeInvincible);
            timeInvincible -= Time.deltaTime;

            if (timeInvincible <= 0.0f) OnEndInvincible.Invoke();
        }
    }

    public override void Damage(int delta)
    {
        if (timeInvincible > 0.0f) return;
        base.Damage(delta);
        OnStartInvincible.Invoke();
    }
}
