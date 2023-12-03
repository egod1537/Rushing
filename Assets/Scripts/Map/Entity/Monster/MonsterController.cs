using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterModel))]
public class MonsterController : MonoBehaviour
{
    public float delay;

    MonsterModel _model;
    MonsterModel model
    {
        get
        {
            if (_model == null) _model = GetComponent<MonsterModel>();
            return _model;
        }
    }

    private void Awake()
    {
        StartCoroutine(FSM(UnityEngine.Random.RandomRange(0, 3)));
    }
    IEnumerator FSM(int dir)
    {
        if (model.isDead)
        {
            yield return null;
        }
        else
        {
            MapManager.ins.controller.MoveEntity(model, dir);
            yield return new WaitForSeconds(UnityEngine.Random.RandomRange(0.2f, 1.0f));
            StartCoroutine(FSM(UnityEngine.Random.RandomRange(0, 4)));
        }
    }
}
