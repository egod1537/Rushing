using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIWeaponSlot : MonoBehaviour
{
    [SerializeField] const int UNIT_X = 64;
    [SerializeField] const int UNIT_Y = 48;

    public float duration;
    public Ease ease;
    List<GameObject> slots = new List<GameObject>();
    private void Awake()
    {
        slots = new List<GameObject>();
        int sz = transform.childCount;
        for(int i=0; i < sz;i++)
            slots.Add(transform.GetChild(i).gameObject);
    }
    public void Swap()
    {
        int sz = slots.Count;
        for(int i=0; i < sz; i++)
        {
            int nxt = (i - 1 + sz) % sz;
            slots[i].transform.DOLocalMove(new Vector3(nxt*UNIT_X, -nxt*UNIT_Y), duration).SetEase(ease);
            slots[i].transform.SetSiblingIndex(sz-nxt-1);
        }
        slots.Add(slots[0]);
        slots.RemoveAt(0);
    }
}
