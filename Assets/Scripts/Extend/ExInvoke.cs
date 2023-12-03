using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExInvoke
{
    public static void Invoke(this MonoBehaviour mono, Action action, float time)
    {
        mono.StartCoroutine(InvokeRoutine(action, time));
    }
    private static IEnumerator InvokeRoutine(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
