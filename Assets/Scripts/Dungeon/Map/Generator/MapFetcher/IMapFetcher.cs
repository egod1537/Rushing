using Map.MapGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapFetcher
{
    public void Fetch(MapModel model, MapLayout layout);
}
