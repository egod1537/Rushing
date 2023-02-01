using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _ins;
    public static T ins
    {
        get
        {
            if(_ins == null)
            {
                T finder = FindObjectOfType<T>();
                if (finder == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).Name;
                    _ins = go.AddComponent<T>();
                }
                else _ins = finder;
            }
            return _ins;
        }
    }
}
