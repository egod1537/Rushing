using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExDictionary
{
    public static SerializeDictionary<T1, T2> Seriailize<T1, T2>(this Dictionary<T1, T2> dic){
        SerializeDictionary<T1, T2> ret = new SerializeDictionary<T1, T2>();
        foreach(var p in dic) ret.Add(p.Key, p.Value);
        return ret;
    }
}
