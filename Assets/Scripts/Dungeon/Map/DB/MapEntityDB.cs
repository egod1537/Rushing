using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapEntityDB
{
    private static readonly string DB_PATH = "Map/Entity";

    public static GameObject Load(MapEntityID id)
    {
        var arr = Resources.LoadAll($"{DB_PATH}");

        string target = $"[{(int)id}]{id}";
        foreach (var obj in arr)
            if (obj.name.Equals(target)) return obj as GameObject;
        return null;
    }
    public static MapEntityID GetTreasureID(MapTheme theme)
    {
        switch (theme)
        {
            case MapTheme.Dungeon:
                return MapEntityID.Dungeon_Treasure;
            case MapTheme.GrassLand:
                return MapEntityID.GrassLand_Treasure;
            case MapTheme.Desert:
                return MapEntityID.Desert_Treasure;
            case MapTheme.Nether:
                return MapEntityID.Nether_Treasure;
            case MapTheme.DimensionalRift:
                return MapEntityID.DimensionalRift_Treasure;
        }
        return MapEntityID.Air;
    }
}
