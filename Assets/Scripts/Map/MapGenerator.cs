using MersenneTwister;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
public class MapGenerator
{
    public Dictionary<Vector2Int,GameObject>
        GenerateMapObject(MapResourceTable table, SOMapTheme theme, 
        Dictionary<Vector2Int, MapObjectType> layout)
    {
        int n = table.n;
        Dictionary<Vector2Int, GameObject> map = new Dictionary<Vector2Int, GameObject>();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                map.Add(new Vector2Int(i, j), theme.rand(layout[new Vector2Int(i, j)]).go);
        return map;
    }
    public (Vector2Int start, string path, Dictionary<Vector2Int, MapObjectType>) GenerateMap(
        int nk, MapResourceTable table, ref float progress)
    {
        table.Normalize();
        int n = table.n;

        List<List<MapObjectType>> lt_map = new List<List<MapObjectType>>();
        lt_map.Resize(n, new List<MapObjectType>().Resize(n, MapObjectType.None));

        MT19937 mt = new MT19937();
        int treasure = table.treasureCount;

        for (int k = 0; k < nk || table.treasureCount==treasure; k++)
        {
            Vector2Int pos = new Vector2Int(mt.RandomRange(0, n - 1), mt.RandomRange(0, n - 1));
            MapObjectType temp = lt_map[pos.x][pos.y];

            if (lt_map[pos.x][pos.y] == MapObjectType.Treasure) treasure++;
            lt_map[pos.x][pos.y] = table.rand(mt.genrand_real1());
            if(treasure == 0) 
                while(lt_map[pos.x][pos.y] == MapObjectType.Treasure)
                    lt_map[pos.x][pos.y] = table.rand(mt.genrand_real1());
            if (lt_map[pos.x][pos.y] == MapObjectType.Treasure) treasure--;

            bool ret = CheckMap(lt_map, true);
            progress = 1.0f * k / nk;
            if (ret) continue;
            else lt_map[pos.x][pos.y] = temp;
        }

        Dictionary<Vector2Int, MapObjectType> map = new Dictionary<Vector2Int, MapObjectType>();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++) map.Add(new Vector2Int(i, j), lt_map[i][j]);

        Vector2Int start = FindMapStart(lt_map, true)[0];
        MapTracker tracker = new MapTracker(n,n,start,lt_map);

        return (start, tracker.Track(tracker.Solve()), map); 
    }
    public bool CheckMap(List<List<MapObjectType>> map, bool opt=false)
        => FindMapStart(map, opt).Count > 0;
    public List<Vector2Int> FindMapStart(List<List<MapObjectType>> map, bool opt=false)
    {
        int n = map.Count;
        List<Vector2Int> ans = new List<Vector2Int>();

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                if (map[i][j] != MapObjectType.None) continue;
                if (CheckMapStart(i, j, map))
                {
                    ans.Add(new Vector2Int(i, j));
                    if (opt) return ans;
                }
            }
        return ans;
    }
    public bool CheckMapStart(int sx, int sy, List<List<MapObjectType>> map)
    {
        int n = map.Count;
        MapSolver solver = new MapSolver(n, n, sx, sy, map);
        return solver.Check();
    }
    public List<List<MapObjectType>> ConvertMap(int n, Dictionary<Vector2Int, MapObjectType> dic)
    {
        List<List<MapObjectType>> ans = new List<List<MapObjectType>>();
        ans.Resize(n, new List<MapObjectType>().Resize(n, MapObjectType.None));
        foreach (var p in dic) ans[p.Key.x][p.Key.y] = p.Value;
        return ans;
    }
    public Dictionary<Vector2Int, MapEntity> GenerateEntity(
        MapResourceTable table, Dictionary<Vector2Int, MapObjectType> layout,
        SOMapMonster monster)
    {
        Dictionary<Vector2Int, MapEntity> entity = new Dictionary<Vector2Int, MapEntity>();
        List<Vector2Int> poses = new List<Vector2Int>();
        int cnt = table.monsterCount, n = table.n;
        for (int i=0; i < n; i++)
            for(int j=0; j < n; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                if (layout[pos] != MapObjectType.None) continue;
                poses.Add(pos);
            }
        while (cnt-- > 0)
        {
            int sz = poses.Count;
            int x = UnityEngine.Random.Range(0, sz - 1), y = UnityEngine.Random.Range(0, sz - 1);
            entity.Add(new Vector2Int(x, y), monster.rand().go.GetComponent<MapEntity>());
        }
        return entity;
    }

    public void Print(List<List<MapObjectType>> map)
    {
        int n = map.Count;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            StringBuilder temp = new StringBuilder();
            for (int j = 0; j < n; j++)
            {
                string str = map[i][j].ToString();
                str = new string(' ', 10 - str.Length) + str;
                temp.Append(str + " ");
            }
            sb.AppendLine(temp.ToString());
        }
        Debug.Log(sb.ToString());
    }
}
