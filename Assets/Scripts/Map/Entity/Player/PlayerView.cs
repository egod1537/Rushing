using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    Animator animator;
    PlayerModel model;
    private void Awake()
    {
        model = GetComponent<PlayerModel>();
        animator = GetComponent<Animator>();

        model.OnDamage.AddListener((int delta) =>
        {
            animator.SetTrigger("Damage");
        });
    }
}
