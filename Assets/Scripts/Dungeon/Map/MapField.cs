using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Unity.VisualScripting;

[Serializable]
public class MapField
{
    [Serializable]
    public class TemporaryList : List<Entity> { }

    [SerializeField]
    public int n, m;
    [SerializeField]
    private List<TemporaryList> arr;
    public MapField(int n, int m)
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
            arr[i] = (TemporaryList) new TemporaryList().Resize(m, null);
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

    public Entity this[int x, int y]
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
                if (arr[i][j] == null) sb.Append("Null ");
                else
                    sb.Append(arr[i][j].id.ToString() + " ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
