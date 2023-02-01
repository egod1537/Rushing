using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Unity.VisualScripting;

namespace Map.MapGenerator
{
    public class MapLayout
    {
        private List<List<MapLayoutObject>> objects;

        public int n, m;
        public int count; //보물 개수

        public List<Vector2Int> StartPos = new List<Vector2Int>();
        public List<string> SolvingRoute = new List<string>();
        public MapLayout(int n, int m)
        {
            this.n = n;
            this.m = m;
            objects = new List<List<MapLayoutObject>>();
            objects.Resize(n);
            for (int i = 0; i < n; i++)
            {
                objects[i] = new List<MapLayoutObject>();
                objects[i].Resize(m);
            }
        }
        public MapLayoutObject this[int x, int y]
        {
            get => objects[x][y];
            set => objects[x][y] = value;
        }

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0; i <n; i++)
            {
                for(int j=0; j < m; j++)
                {
                    sb.Append(objects[i][j].ToString() +" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
