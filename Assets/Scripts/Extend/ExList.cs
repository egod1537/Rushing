using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExList
{
    public static List<T> Resize<T>(this List<T> lt, int n, T var)
    {
        lt.Clear();
        for (int i = 0; i < n; i++) lt.Add(var);
        return lt;
    }
    public static List<List<T>> Resize<T>(this List<List<T>> lt, int n, List<T> var)
    {
        lt.Clear();
        for (int i = 0; i < n; i++) lt.Add(new List<T>(var));
        return lt;
    }
    public static List<T> Resize<T>(this List<T> lt, int n)
        => lt.Resize(n, default(T));
}
