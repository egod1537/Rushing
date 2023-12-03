using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamageIndicator
{
    const string SKIN_PATH = "VFX/Damage";
    const float INDICATE_TIME = 0.5f;
    const float INDICATE_Y = 100f;

    private static GameObject _canvas;
    public static GameObject canvas
    {
        get
        {
            if (_canvas == null)
                _canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
            return _canvas;
        }
    }

    private static GameObject _vfx;
    public static GameObject vfx
    {
        get
        {
            if (_vfx == null)
                _vfx = Resources.Load(SKIN_PATH) as GameObject;
            return _vfx;
        }
    }

    public static void Show(MapEntity entity, int damage)
    {
        GameObject go = Object.Instantiate(vfx);

        Transform tr = go.transform;
        tr.SetParent(canvas.transform);
        tr.localPosition = Camera.main.WorldToScreenPoint(entity.transform.position) 
            -  new Vector3(Screen.width, Screen.height)*0.5f;

        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = damage.ToString();

        Vector3 pos = tr.localPosition;
        Sequence seq = DOTween.Sequence();
        seq.Append(tr.DOLocalMoveY(pos.y + INDICATE_Y, INDICATE_TIME / 2));
        seq.Append(tr.DOLocalMoveY(pos.y + INDICATE_Y / 2, INDICATE_TIME / 2));
        seq.AppendCallback(() => { Object.Destroy(go); });
        seq.Restart();
    }
}
