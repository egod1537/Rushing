using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace Map.MapGenerator
{
    public class ConvertMapSolver
    {
        public static List<string> ToMapSolver(int startX, int startY, MapLayout layout)
        {
            int n = layout.n;
            List<string> ans = new List<string>();
            for (int i = 0; i < n; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < n; j++)
                {
                    if (i == startX && j == startY) sb.Append('O');
                    else
                    {
                        if (layout[i,j] == MapLayoutObject.Block) sb.Append('#');
                        else if (layout[i,j] == MapLayoutObject.Key) sb.Append('*');
                        else sb.Append('.');
                    }
                }
                ans.Add(sb.ToString());
            }

            //StringBuilder tt = new StringBuilder();
            //foreach (string str in ans) tt.AppendLine(str);
            //Debug.Log(tt.ToString());

            return ans;
        }
        public static List<string> ToMapSolver(Vector2Int pos, MapLayout layout)
            => ToMapSolver(pos.x, pos.y, layout);
    }
}
