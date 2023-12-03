using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public static class ExSpriteGroup
{
    public static TweenerCore<float, float, FloatOptions> DOFade(this SpriteGroup target, float endValue, float duration)
    {
        TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.alpha, x => target.alpha = x, endValue, duration);
        t.SetTarget(target);
        return t;
    }
}
