using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterModel))]
public class MonsterView : MonoBehaviour
{
    MonsterModel model;

    Animator animator;
    GameObject graphic;
    SpriteRenderer spriteMonster;

    UIMonster uiMonster;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        model = GetComponent<MonsterModel>();
        graphic = transform.FindChild("graphic").gameObject;
        spriteMonster = graphic.GetComponent<SpriteRenderer>();
        uiMonster = transform.FindChild("UI").GetComponent<UIMonster>();

        uiMonster.uiHpBar.SetRatio(1.0f*model.hp / model.maxHp);

        model.OnMove.AddListener((Direction dir) =>
        {
            if (dir == Direction.Left) spriteMonster.flipX = false;
            if (dir == Direction.Right) spriteMonster.flipX = true;

            animator.speed = model.speed;
            animator.SetTrigger("Move");
        });

        model.OnDead.AddListener(() =>
        {
            animator.speed = 1.0f;
            animator.Play("Monster@Dead");
        });

        model.OnEditHp.AddListener(() =>
        {
            uiMonster.uiHpBar.SetRatio(1.0f*model.hp / model.maxHp);
        });

        model.OnDamage.AddListener((int delta) =>{
            animator.speed = 1.0f;
            animator.SetTrigger("Damage");      
        });
    }
}
