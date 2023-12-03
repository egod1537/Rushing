using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapSummoner
{
    protected MapManager model { get { return MapManager.ins; } }
    protected MapController controller { get { return model.controller; } }

    public abstract GameObject Summon(Vector2Int pos, GameObject go, bool isCreated=false);
    public abstract void Destroy(Vector2Int pos, GameObject go);
    public abstract void DestroyAll();
}
