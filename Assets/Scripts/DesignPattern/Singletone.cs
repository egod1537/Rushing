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
            if(!_ins)
            {
                _ins = (T)FindObjectOfType(typeof(T));
                if (!_ins)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _ins = go.AddComponent<T>();
                }
            }
            return _ins;
        }
    }
}
