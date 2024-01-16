using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MapSolver
{
    public int n, m;
    public List<string> arr;

    private int idx;
    public MapSolver(int _n, int _m)
    {
        n = _n; m = _m;
        idx = 0;
        arr = new List<string>();
        arr.Resize(n, "");
    }
    public MapSolver(int _n, int _m, int sx, int sy, List<List<MapObjectType>> _arr) : this(_n, _m)
    {
        ReadMap(ConvertMap(sx, sy, _arr));
    }
    public MapSolver(int _n, int _m, Vector2Int start, List<List<MapObjectType>> _arr) : this(_n, _m)
    {
        ReadMap(ConvertMap(start, _arr));
    }
    public MapSolver(int _n, int _m, List<string> _arr) : this(_n, _m)
    {
        ReadMap(_arr);
    }

    protected List<List<int>> num;
    protected List<Vector2Int> inum;
    protected List<Vector2Int> star = new List<Vector2Int>();
    public void ReadMap(List<string> _arr)
    {
        num = new List<List<int>>();
        num.Resize(n, new List<int>().Resize(m));

        inum = new List<Vector2Int>();
        inum.Resize(n * m + 1, new Vector2Int(0, 0));

        arr = new List<string>(_arr);

        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
            {
                if (arr[i][j] != '#')
                {
                    num[i][j] = ++idx;
                    inum[num[i][j]] = new Vector2Int(i, j);
                }
                if (arr[i][j] == 'O')
                {
                    sx = i; sy = j;
                }
                else if (arr[i][j] == '*') star.Add(new Vector2Int(i, j));
            }
    }

    protected int sx, sy;

    protected List<List<int>>[] dp;
    protected int[] dx = { 1, 0, -1, 0 };
    protected int[] dy = { 0, 1, 0, -1 };

    /// <summary>
    /// Tilt Maze 맵을 그래프로 표현하기 위해 DP를 사용하여 필요한 정보 전처리하는 함수
    /// </summary>
    private void FillDP()
    {
        dp = new List<List<int>>[4];
        for (int i = 0; i < 4; i++)
        {
            dp[i] = new List<List<int>>();
            dp[i].Resize(n, new List<int>().Resize(m));
        }

        int tn = n, tm = m;
        for (int k = 0; k < 4; k++)
        {
            for (int i = 0; i < tm; i++)
            {
                int u = tn - 1, v = i;
                if (k > 1) u = 0;
                if (k % 2 == 1) (u, v) = (v, u);
                if (num[u][v] > 0) dp[k][u][v] = num[u][v];
            }
            List<int> lt = new List<int>();
            for (int i = 0; i < tn - 1; i++) lt.Add(i);
            if (k < 2) lt.Reverse();
            if (k > 1)
                for (int i = 0; i < lt.Count; i++) lt[i]++;
            foreach (int i in lt)
            {
                for (int j = 0; j < tm; j++)
                {
                    int ni = i, nj = j;
                    if (k % 2 == 1) (ni, nj) = (nj, ni);
                    if (arr[ni][nj] != '#')
                    {
                        dp[k][ni][nj] = num[ni][nj];
                        if (dp[k][ni + dx[k]][nj + dy[k]] > 0)
                            dp[k][ni][nj] = dp[k][ni + dx[k]][nj + dy[k]];
                    }
                }
            }
            (tn, tm) = (tm, tn);
        }
    }

    protected SCC scc;
    /// <summary>
    /// SCC 만드는 함수
    /// </summary>
    private void ConstructSCC()
    {
        scc = new SCC(idx + 1);
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
            {
                if (arr[i][j] == '#') continue;
                for (int k = 0; k < 4; k++)
                    scc.add_edge(num[i][j], dp[k][i][j]);
            }
        scc.Create();
    }
    protected int SCCGroup(int g) => scc.group[g];
    protected int SCCGroup(int x, int y) => SCCGroup(num[x][y]);

    /// <summary>
    /// SCC로 압축된 그래프에서 2-SAT의 해를 찾는 과정
    /// </summary>
    /// <returns> 각 변수의 값, 실패하는 경우 -1이 포함된다. </returns>
    public List<int> Solve()
    {
        FillDP();
        ConstructSCC();

        int sz = scc.sccCount;
        Graph compress = scc.Compress();

        int st = SCCGroup(sx, sy);

        TwoSat ts = new TwoSat(sz);
        ts.add_clause(st, st);

        List<List<bool>> reach = new List<List<bool>>();
        reach.Resize(sz, new List<bool>().Resize(sz, false));

        for (int i = 0; i < sz; i++)
            reach[i] = new List<bool>(compress.BFS(i));

        for (int i = 0; i < sz; i++)
        {
            if (!reach[st][i]) ts.add_clause(i, i, true, true);
            else
            {
                for (int j = 0; j < sz; j++)
                {
                    if (!reach[st][j]) continue;
                    if (!reach[i][j] && !reach[j][i]) ts.add_clause(i, j, true, true);
                }
            }
        }

        foreach (Vector2Int vec in star)
        {
            int u = dp[0][vec.x][vec.y], v = dp[1][vec.x][vec.y];
            ts.add_clause(SCCGroup(u), SCCGroup(v));
        }

        return ts.calc();
    }
    public bool Check()
    {
        List<int> ret = Solve();
        return ret[0] != -1;
    }

    /// <summary>
    /// 맵 레이아웃을 다루기 편한 형태로 변환하는 함수
    /// </summary>
    /// <param name="sx"> 시작 위치 x </param>
    /// <param name="sy"> 시작 위치 y </param>
    /// <param name="map"> 맵 레이아웃 </param>
    /// <returns></returns>
    public List<string> ConvertMap(int sx, int sy, List<List<MapObjectType>> map)
    {
        int n = map.Count;
        List<string> ans = new List<string>();
        for (int i = 0; i < n; i++)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < n; j++)
            {
                if (i == sx && j == sy) sb.Append('O');
                else
                {
                    if (map[i][j] == MapObjectType.Block) sb.Append('#');
                    else if (map[i][j] == MapObjectType.Treasure) sb.Append('*');
                    else sb.Append('.');
                }
            }
            ans.Add(sb.ToString());
        }
        return ans;
    }
    public List<string> ConvertMap(Vector2Int pos, List<List<MapObjectType>> map)
        => ConvertMap(pos.x, pos.y, map);
}
