using MersenneTwister;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

namespace Map.MapGenerator
{
    public class MapCreator : IMapCreator
    {
        private IMapValidator validator;
        public MapCreator(IMapValidator validator)
        {
            this.validator = validator;
        }

        public MapLayout Create(
            int n, 
            int m, 
            int key,
            MapProbabilityTable table,
            int iteration,
            ref float progress)
        {
            table.Normalize();
            MT19937 mt = new MT19937();
            int rand(int l, int r) { return mt.RandomRange(l, r); }

            MapLayout layout = new MapLayout(n, m);

            int keyCount = 0;
            for(int t=0; t < iteration || keyCount < key || keyCount == 0; t++)
            {
                int x = rand(0, n - 1), y = rand(0, m - 1);
                MapLayoutObject type = table.layout.get();
                if (type == MapLayoutObject.Key && keyCount == key) continue;

                if (layout[x, y] == MapLayoutObject.Key) keyCount--;
                layout[x, y] = type;
                if (validator.Check(layout))
                {
                    if (type == MapLayoutObject.Key) keyCount++;
                }else layout[x,y] = MapLayoutObject.None;

                progress = 1.0f*t / iteration;
            }

            for(int i=0; i < n; i++)
                for(int j=0; j < m; j++)
                {
                    if (layout[i, j] != MapLayoutObject.None) continue;
                    MapTracker tracker = new MapTracker(i,j,layout);
                    if (tracker.Check())
                    {
                        layout.StartPos.Add(new Vector2Int(i, j));
                        layout.SolvingRoute.Add(tracker.Track(tracker.Solve()));
                    }
                }

            progress = 1.0f;
            return layout;
        }
    }
}
