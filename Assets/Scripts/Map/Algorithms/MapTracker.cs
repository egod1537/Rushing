using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 주어진 맵의 해결 방법을 찾는 클래스
/// </summary>
public class MapTracker : MapSolver
{
    public MapTracker(int _n, int _m) : base(_n, _m)
    {
    }
    public MapTracker(int _n, int _m, int sx, int sy, List<List<MapObjectType>> _arr) : this(_n, _m)
    {
        ReadMap(ConvertMap(sx, sy, _arr));
    }
    public MapTracker(int _n, int _m, Vector2Int start, List<List<MapObjectType>> _arr) : this(_n, _m)
    {
        ReadMap(ConvertMap(start, _arr));
    }
    public MapTracker(int _n, int _m, List<string> _arr) : base(_n, _m, _arr)
    {
    }

    private char Dir(Vector2Int from, Vector2Int to)
    {
        List<char> txt = new List<char>() { 'D', 'R', 'U', 'L' };
        int nx = to.x - from.x, ny = to.y - from.y;
        int tx = (nx == 0 ? 0 : nx / Math.Abs(nx)), ty = (ny == 0 ? 0 : ny / Math.Abs(ny));

        for (int i = 0; i < 4; i++)
            if (tx == dx[i] && ty == dy[i])
                return txt[i];
        return ' ';
    }

    /// <summary>
    /// SCC로 압축된 그래프에서 경로를 찾는 함수
    /// </summary>
    /// <param name="ret"> 방문해야하는 정점의 순서 </param>
    /// <returns></returns>
    public List<int> FindRoute(List<int> ret)
    {
        int sz = ret.Count, cnt = 0;
        List<int> ind = new List<int>();
        ind.Resize(sz, 0);

        Graph compress = scc.Compress();
        for (int i = 0; i < sz; i++)
            foreach (int j in compress.E[i]) ind[j]++;
        List<int> temp = new List<int>(ind);

        List<bool> chk = new List<bool>();
        chk.Resize(sz, false);
        for (int i = 0; i < sz; i++)
            if (ret[i] == 1) chk[i] = true;

        List<int> lv = new List<int>();
        lv.Resize(sz, -1);

        //위상 정렬로 탐색 순서 정하기
        Queue<int> q = new Queue<int>();
        for (int i = 0; i < sz; i++)
            if (ind[i] == 0) q.Enqueue(i);

        while (q.Count > 0)
        {
            int top = q.Dequeue();
            if (chk[top]) lv[top] = cnt++;
            foreach (int v in compress.E[top])
                if (--ind[v] == 0) q.Enqueue(v);
        }

        //BFS 돌면서 최단 경로 찾기
        ind = new List<int>(temp);
        List<int> dp = new List<int>();
        List<int> prv = new List<int>();
        dp.Resize(sz, -1);
        prv.Resize(sz, -1);
        for (int i = 0; i < sz; i++)
            if (ind[i] == 0) q.Enqueue(i);
        dp[SCCGroup(sx, sy)] = 0;
        while (q.Count > 0)
        {
            int top = q.Dequeue();
            foreach (int v in compress.E[top])
            {
                if (dp[top] + 1 == lv[v])
                {
                    dp[v] = lv[v];
                    prv[v] = top;
                }
                else if (dp[v] < dp[top])
                {
                    dp[v] = dp[top];
                    prv[v] = top;
                }
                if (--ind[v] == 0) q.Enqueue(v);
            }
        }

        //구체적인 경로 찾기
        List<int> route = new List<int>();
        for (int i = 0; i < sz; i++)
        {
            if (lv[i] != cnt - 1) continue;
            int now = i;
            while (now != SCCGroup(sx, sy))
            {
                route.Add(now);
                now = prv[now];
            }
            route.Add(now);
            break;
        }
        route.Reverse();
        return route;
    }

    /// <summary>
    /// 방문해야하는 정점의 순서는 상하좌우으로 변환하는 함수.
    /// </summary>
    /// <param name="ret"></param>
    /// <returns></returns>
    public string Track(List<int> ret)
    {
        List<int> route = FindRoute(ret);

        List<List<int>> dst = new List<List<int>>();
        List<List<string>> path = new List<List<string>>();
        dst.Resize(scc.n, new List<int>().Resize(scc.n, -1));
        path.Resize(scc.n, new List<string>().Resize(scc.n, ""));
        foreach (int i in route) DstInSCC(i, route, dst, path);

        return Routing(route, dst, path);
    }

    /// <summary>
    /// SCC의 정점에서 모든 보석을 먹는 최단 경로를 찾는 함수
    /// </summary>
    /// <param name="sc"> SCC 정점의 번호 </param>
    /// <param name="route"></param>
    /// <param name="dst"></param>
    /// <param name="path"></param>
    private void DstInSCC(int sc, 
                          List<int> route,
                          List<List<int>> dst, 
                          List<List<string>> path)
    {
        Dictionary<int, int> id = new Dictionary<int, int>();
        List<int> g = scc.scc[sc];
        int sz = g.Count;
        for (int i = 0; i < sz; i++) id.Add(g[i], i);

        Dictionary<int, int> road = StarInSCC(sc, route);
        int starCnt = road.Count;

        foreach (int st in g)
        {
            List<List<int>> dp = new List<List<int>>();
            List<List<Vector2Int>> prv = new List<List<Vector2Int>>();
            dp.Resize(sz, new List<int>().Resize(1 << starCnt, -1));
            prv.Resize(sz, new List<Vector2Int>().Resize(1 << starCnt, new Vector2Int(-1, -1)));

            Queue<Vector2Int> q = new Queue<Vector2Int>();

            int gost = 0;
            if (road.ContainsKey(st)) gost |= (1 << road[st]);
            dp[id[st]][gost] = 0;
            q.Enqueue(new Vector2Int(id[st], gost));

            while (q.Count > 0)
            {
                Vector2Int top = q.Dequeue();
                int u = top.x, state = top.y;
                Vector2Int pos = inum[g[u]];
                for (int i = 0; i < 4; i++)
                {
                    int nx = pos.x, ny = pos.y;
                    int tst = state;
                    while (0 <= nx + dx[i] && nx + dx[i] < n &&
                        0 <= ny + dy[i] && ny + dy[i] < m &&
                        arr[nx + dx[i]][ny + dy[i]] != '#')
                    {
                        nx += dx[i]; ny += dy[i];
                        int idx = num[nx][ny];
                        if (road.ContainsKey(idx)) tst |= (1 << road[idx]);
                    }
                    int to = num[nx][ny];
                    if (SCCGroup(to) == sc && dp[id[to]][tst] == -1)
                    {
                        dp[id[to]][tst] = dp[u][state] + 1;
                        prv[id[to]][tst] = top;
                        q.Enqueue(new Vector2Int(id[to], tst));
                    }
                }
            }
            foreach (int u in g)
            {
                dst[st][u] = dp[id[u]][(1 << starCnt) - 1];
                StringBuilder sb = new StringBuilder();
                int pos = id[u], state = (1 << starCnt) - 1;
                while (pos != id[st] || state != gost)
                {
                    Vector2Int v = prv[pos][state];
                    sb.Append(Dir(inum[g[v.x]], inum[g[pos]]));
                    pos = v.x; state = v.y;
                }
                path[st][u] = sb.ToString().Reverse();
            }
        }
    }
    private Dictionary<int, int> StarInSCC(int sc, List<int> route)
    {
        Dictionary<int, int> ans = new Dictionary<int, int>();
        Dictionary<int, int> lv = new Dictionary<int, int>();
        for (int i = 0; i < route.Count; i++) lv.Add(route[i], i);
        int cnt = 0;
        foreach (Vector2Int s in star)
        {
            List<int> lt = new List<int>();
            int t = SCCGroup(dp[0][s.x][s.y]);
            if (lv.ContainsKey(t)) lt.Add(t);
            t = SCCGroup(dp[1][s.x][s.y]);
            if (lv.ContainsKey(t)) lt.Add(t);
            if (lt.Count == 2 && lv[lt[0]] > lv[lt[1]])
                (lt[0], lt[1]) = (lt[1], lt[0]);
            if (lt[0] == sc)
                ans.Add(num[s.x][s.y], cnt++);
        }
        return ans;
    }

    /// <summary>
    /// 맵의 클리어 가능한 경로를 찾는 함수
    /// </summary>
    /// <param name="route"></param>
    /// <param name="dst"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private string Routing(List<int> route, List<List<int>> dst, List<List<string>> path)
    {
        List<int> lv = new List<int>();
        lv.Resize(scc.sccCount, -1);
        for (int i = 0; i < route.Count; i++)
            lv[route[i]] = i;

        Dictionary<int, int> id = new Dictionary<int, int>();
        int sz = route.Count;
        for (int i = 0; i < sz; i++) id.Add(route[i], i);

        List<List<int>>[] dp = new List<List<int>>[2];
        List<List<int>>[] prv = new List<List<int>>[2];
        for (int i = 0; i < 2; i++)
        {
            dp[i] = new List<List<int>>();
            prv[i] = new List<List<int>>();
            dp[i].Resize(sz, new List<int>());
            prv[i].Resize(sz, new List<int>());
        }

        List<Dictionary<int, int>> nid = new List<Dictionary<int, int>>();
        nid.Resize(sz, new Dictionary<int, int>());
        for (int i = 0; i < sz; i++)
        {
            List<int> lt = scc.scc[route[i]];
            for (int j = 0; j < lt.Count; j++) nid[i].Add(lt[j], j);
            for (int j = 0; j < 2; j++)
            {
                dp[j][i].Resize(lt.Count, 0x3f3f3f3f);
                prv[j][i].Resize(lt.Count, -1);
            }
        }
        Dictionary<Vector2Int, bool> con = new Dictionary<Vector2Int, bool>();
        for (int i = 0; i < sz; i++)
            foreach (int j in scc.scc[route[i]])
                foreach (int k in scc.E[j])
                    if (i + 1 == lv[SCCGroup(k)])
                        con[new Vector2Int(j, k)] = true;

        dp[0][0][nid[0][num[sx][sy]]] = 0;
        for (int i = 0; i < sz; i++)
        {
            if (i > 0)
            {
                foreach (int u in scc.scc[route[i]])
                    foreach (int v in scc.scc[route[i - 1]])
                    {
                        if (!con.ContainsKey(new Vector2Int(v, u))) continue;
                        int d = dp[1][i - 1][nid[i - 1][v]] + 1;
                        if (dp[0][i][nid[i][u]] > d)
                        {
                            dp[0][i][nid[i][u]] = d;
                            prv[0][i][nid[i][u]] = v;
                        }
                    }
            }
            foreach (int u in scc.scc[route[i]])
                foreach (int v in scc.scc[route[i]])
                {
                    int d = dp[0][i][nid[i][v]] + dst[v][u];
                    if (dp[1][i][nid[i][u]] > d)
                    {
                        dp[1][i][nid[i][u]] = d;
                        prv[1][i][nid[i][u]] = v;
                    }
                }
        }


        int ans = 1 << 30;
        foreach (int var in dp[1][sz - 1]) ans = Math.Min(ans, var);
        List<string> ro = new List<string>();
        for (int i = 0; i < dp[1][sz - 1].Count; i++)
        {
            if (ans != dp[1][sz - 1][i]) continue;
            int f = 1, sc = sz - 1, pos = scc.scc[route[sz - 1]][i];
            while (!(f == 0 && sc == 0))
            {
                int b = prv[f][sc][nid[sc][pos]];
                if (f == 1)
                {
                    ro.Add(path[b][pos]);
                    f = 0; pos = b;
                }
                else
                {
                    ro.Add(Dir(inum[b], inum[pos]).ToString());
                    f = 1; sc--; pos = b;
                }

            }
            break;
        }
        ro.Reverse();
        StringBuilder sb = new StringBuilder();
        foreach (string str in ro) sb.Append(str);
        return sb.ToString();
    }
}