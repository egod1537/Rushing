using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGroup : MonoBehaviour
{
    [SerializeField]
    private float _alpha;
    public float alpha
    {
        get { return _alpha; }
        set
        {
            _alpha = value;
            ChangeAlpha(this.transform, value);
        }
    }

    private Dictionary<Transform, SpriteRenderer> spriteRenderes = new Dictionary<Transform, SpriteRenderer>();

    public void ChangeAlpha(Transform now, float value)
    {
        if (spriteRenderes.ContainsKey(now))
        {
            SpriteRenderer spriteRenderer = spriteRenderes[now];
            if (spriteRenderes[now] != null)
            {
                Color col = spriteRenderer.color;
                col.a = value;
                spriteRenderer.color = col;
            }
        }
        else
            spriteRenderes.Add(now, now.GetComponent<SpriteRenderer>());
        int sz = now.childCount;
        for (int i = 0; i < sz; i++)
            ChangeAlpha(now.GetChild(i), value);
    }
}
