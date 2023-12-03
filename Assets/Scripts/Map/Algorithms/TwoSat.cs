using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSat : SCC
{
    public int size;
    public TwoSat(int _n) : base(2 * _n)
    {
        size = _n;
    }
    private int inv(int x) { return x + size; }
    public void add_clause(int u, int v, bool arev = false, bool brev = false)
    {
        int f1 = arev ? inv(u) : u, t1 = brev ? v : inv(v);
        int f2 = brev ? inv(v) : v, t2 = arev ? u : inv(u);
        add_edge(f1, t1);
        add_edge(f2, t2);
    }
    public List<int> calc()
    {
        Create();

        bool ok = true;
        for (int i = 0; i < size; i++)
            if (group[i] == group[inv(i)]) ok = false;
        List<int> ans = new List<int>();
        ans.Resize(size, -1);
        if (ok)
            for (int i = 0; i < size; i++)
                ans[i] = Convert.ToInt32(group[i] > group[inv(i)]);
        return ans;
    }
}
