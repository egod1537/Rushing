using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.MapGenerator
{
    public class MapValidator : IMapValidator
    {
        public bool Check(MapLayout layout)
        {
            int n = layout.n, m = layout.m;
            for (int i = 0; i < n; i++)
            {
                for(int j=0; j < m; j++)
                {
                    if (layout[i, j] != MapLayoutObject.None) continue;

                    MapSolver solver = new MapSolver(i, j, layout);
                    if (solver.Check()) return true;
                }
            }
            return false;
        }
    }
}
