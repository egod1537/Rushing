using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapSpawner
{
    public void Spawn(MapModel model);
    public void SpawnPlayer(MapModel model);
}
