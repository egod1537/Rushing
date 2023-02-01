using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class ExList
{
    public static List<T> Resize<T>(this List<T> list, int n, T var)
    {
        int count = list.Count;

        if (n < count)
        {
            list.RemoveRange(n, count - n);
        }
        else if (n > count)
        {
            if (n > list.Capacity)   // Optimization
                list.Capacity = n;

            list.AddRange(Enumerable.Repeat(var, n - count));
        }

        return list;
    }
    public static List<T> Resize<T>(this List<T> lt, int n) 
        => lt.Resize(n, default(T));
    public static List<List<T>> Resize<T>(this List<List<T>> list, int n, int m, T var)
    {
        int count = list.Count;

        if (n < count)
        {
            list.RemoveRange(n, count - n);
        }
        else if (n > count)
        {
            if (n > list.Capacity)   // Optimization
                list.Capacity = n;

            for (int i = 0; i < n - count; i++)
                list.Add(new List<T>().Resize(m, var));
        }

        return list;
    }
    public static List<List<T>> Resize<T>(this List<List<T>> lt, int n, int m)
        => lt.Resize(n, m, default(T));
}
