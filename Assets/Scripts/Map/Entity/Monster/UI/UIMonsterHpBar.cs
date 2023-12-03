using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMonsterHpBar : MonoBehaviour
{
    const float DURATION = 0.5f;

    public SpriteRenderer bar;
    public float ratio;

    SpriteGroup group;

    private void Awake()
    {
        group = GetComponent<SpriteGroup>();
        group.alpha = 0.0f;
        SetRatio(ratio);
    }

    public void SetRatio(float r)
    {
        r = Mathf.Clamp01(r);

        bar.transform.DOKill();
        bar.DOKill();
        group.DOKill();

        group.DOFade(1.0f, 0.1f)
            .OnComplete(() => {
                bar.transform.DOScaleX(r, DURATION);

                Color color = bar.color;
                bar.DOColor(Color.black, DURATION)
                    .OnComplete(() => {
                        bar.DOColor(color, DURATION / 2);
                        group.DOFade(0.0f, 0.1f).SetDelay(0.5f);
                    });
            });
        ratio = r;
    }
}
