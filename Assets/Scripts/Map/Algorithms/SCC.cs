using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Tarjan 알고리즘을 사용하여 SCC를 구하는 클래스
/// </summary>
public class SCC : Graph
{
    public int sccCount;
    public List<List<int>> scc;
    public List<int> group;

    private Stack<int> stk;
    private List<bool> fin;
    private List<int> dfsn;

    private int cnt;

    public SCC(int _n) : base(_n)
    {
        init();
    }
    private void init()
    {
        sccCount = cnt = 0;
        fin = new List<bool>();
        fin.Resize(n, false);

        scc = new List<List<int>>();

        group = new List<int>();
        group.Resize(n, -1);

        dfsn = new List<int>();
        dfsn.Resize(n, 0);

        stk = new Stack<int>();
    }
    private int dfs(int u)
    {
        dfsn[u] = ++cnt;
        stk.Push(u);

        int ret = dfsn[u];
        foreach (int v in E[u])
        {
            if (dfsn[v] == 0) ret = Math.Min(ret, dfs(v));
            else if (!fin[v]) ret = Math.Min(ret, dfsn[v]);
        }
        if (ret == dfsn[u])
        {
            List<int> now = new List<int>();
            while (true)
            {
                int top = stk.Pop();
                now.Add(top);
                fin[top] = true;
                group[top] = sccCount;
                if (top == u) break;
            }
            scc.Add(now);
            sccCount++;

        }
        return ret;
    }

    public void Create()
    {
        init();

        for (int i = 0; i < n; i++)
            if (dfsn[i] == 0) dfs(i);
    }

    public Graph Compress()
    {
        int sz = sccCount;
        Graph ans = new Graph(sz);
        for (int i = 0; i < n; i++)
        {
            foreach (int j in E[i])
            {
                int u = group[i], v = group[j];
                if (u != v) ans.add_edge(u, v);
            }
        }
        for (int i = 0; i < sz; i++)
            ans.E[i] = ans.E[i].Distinct().ToList();
        return ans;
    }
}
