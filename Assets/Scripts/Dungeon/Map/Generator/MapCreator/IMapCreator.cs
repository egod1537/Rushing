using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Map.MapGenerator
{
    public interface IMapCreator
    {
        public MapLayout Create(
            int n, 
            int m, 
            int keyCount,
            MapProbabilityTable table,
            int iteration,
            ref float progress);
    }
}
