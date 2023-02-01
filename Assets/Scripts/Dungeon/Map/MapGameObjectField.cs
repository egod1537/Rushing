using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class MapGameObjectField
{
    [Serializable]
    public class TemporaryList : List<GameObject> { }

    public int n, m;
    [SerializeField]
    public List<TemporaryList> arr;
    public MapGameObjectField(int n, int m)
    {
        this.n = n;
        this.m = m;
        Clear();
    }
    public void Clear()
    {
        arr = new List<TemporaryList>();
        arr.Resize(n);
        for (int i = 0; i < n; i++)
        {
            arr[i] = new TemporaryList();
            arr[i].Resize(m, null);
        }
    }
    public void Resize(int n, int m)
    {
        this.n = n;
        this.m = m;
        arr.Resize(n);
        for (int i = 0; i < n; i++)
        {
            if (arr[i] == null)
                arr[i] = new TemporaryList();
            arr[i].Resize(m);
        }
    }

    public GameObject this[int x, int y]
    {
        get => arr[x][y];
        set => arr[x][y] = value;
    }

    public string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                sb.Append(arr[i][j].ToString() + " ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
