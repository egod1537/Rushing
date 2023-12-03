using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class MonsterObject
{
    [SerializeField]
    public GameObject go;
    [SerializeField]
    public float percent;
}
[CreateAssetMenu(fileName = "SOMapMonster", menuName = "Rushing/Map Monster", order = int.MaxValue)]
public class SOMapMonster : MonoBehaviour
{
    public MonsterObject[] monster;
    public MonsterObject rand()
    {
        float t = UnityEngine.Random.Range(0.0f, 1.0f);
        float sum = 0.0f;
        foreach (var o in monster)
        {
            if (sum <= t && t <= sum + o.percent)
                return o;
            sum += o.percent;
        }
        return new MonsterObject();
    }

    public void NormalizePercent(MapObjectType target)
    {
        float sum = 0.0f;
        foreach (var t in monster) sum += t.percent;
        if (Mathf.Approximately(sum, 0.0f)) return;
        int sz = monster.Length;
        for (int i = 0; i < sz; i++) monster[i].percent /= sum;
    }
}
