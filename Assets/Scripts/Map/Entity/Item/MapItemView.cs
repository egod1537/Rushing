using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapItemView : MonoBehaviour
{
    public MapItem model;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        model = GetComponent<MapItem>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();

        model.entity.OnDead.AddListener(() =>
        {
            Vector3 pos = transform.localPosition;
            transform.DOLocalMoveY(pos.y + 0.5f, 0.25f)
            .OnComplete(() =>
            {
                transform.DOLocalMoveY(pos.y + 0.25f, 0.25f);
                spriteRenderer.DOFade(0f, 0.25f);
            });
        });
    }
}
