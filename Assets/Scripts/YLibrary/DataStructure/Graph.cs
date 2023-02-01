using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public int n;
    public List<List<int>> E;
    public Graph(int _n)
    {
        n = _n;
        E = new List<List<int>>();
        E.Resize(n, 0);
    }
    public void add_edge(int u, int v, bool bi = false)
    {
        E[u].Add(v);
        if (bi) E[v].Add(u);
    }
    public List<bool> BFS(int s)
    {
        Queue<int> q = new Queue<int>();
        List<bool> vit = new List<bool>();
        vit.Resize(n, false);
        q.Enqueue(s); vit[s] = true;
        while (q.Count > 0)
        {
            int top = q.Dequeue();
            foreach (int t in E[top])
            {
                if (vit[t]) continue;
                vit[t] = true;
                q.Enqueue(t);
            }
        }
        return vit;
    }
}