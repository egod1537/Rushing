using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Entity))]
public class EntityView : MonoBehaviour
{
    private Entity _entity;
    public Entity entity { get => _entity ??= GetComponent<Entity>(); }

    private Animator _animator;
    public Animator animator { get => _animator ??= GetComponent<Animator>(); }

    private void Awake()
    {
        entity.OnMove.AddListener((Vector2Int delta) =>
        {
            animator.SetFloat("MoveX", delta.x);
            animator.SetFloat("MoveY", delta.y);

            animator.Play("Rush");
        });
    }
}
