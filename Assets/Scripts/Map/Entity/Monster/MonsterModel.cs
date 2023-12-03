using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MapEntity
{
    private void Awake()
    {
        OnDamage.AddListener((int delta) =>
        {

        });

        OnConfilct.AddListener((MapEntity other) =>
        {
            if (other.entityType == MapEntityType.Player)
                Attack(other);
        });

        OnMove.AddListener((Direction dir) =>
        {
            lookDirection = dir;
        });
    }
}
