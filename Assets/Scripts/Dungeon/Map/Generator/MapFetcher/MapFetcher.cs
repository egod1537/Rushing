using Map.MapGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFetcher : IMapFetcher
{
    public void Fetch(MapModel model, MapLayout layout)
    {
        MapTheme theme = DungeonManager.ins.Theme;
        int floor = DungeonManager.ins.Floor;

        MapProbabilityTable table = MapDB.LoadProbabilityTable(theme, floor);

        int n = model.Size.x, m = model.Size.y;
        for(int i=0; i < n; i++)
            for(int j=0; j < m; j++)
            {
                if (layout[i, j] == MapLayoutObject.Block)
                {
                    MapEntityID id = table.Rand(MapEntityLayer.GroundStructure);
                    MapBrain.ins.Create(MapEntityLayer.GroundStructure
                        ,new Vector2Int(i, j)
                        ,id);
                }
                else if (layout[i,j] == MapLayoutObject.Key)
                {
                    MapEntityID id = MapEntityDB.GetTreasureID(theme);
                    MapBrain.ins.Create(MapEntityLayer.Ground
                        , new Vector2Int(i, j)
                        , id);
                }
            }
    }
}
