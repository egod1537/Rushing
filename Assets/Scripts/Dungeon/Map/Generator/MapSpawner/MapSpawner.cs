using MersenneTwister;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : IMapSpawner
{
    private HashSet<Vector2Int> _visit;
    public MapSpawner()
    {
        _visit = new HashSet<Vector2Int>();
    }

    public void Spawn(MapModel model)
    {
        MapEntityLayer layer = MapEntityLayer.GroundEntity;
        MapTheme theme = DungeonManager.ins.Theme;
        int floor = DungeonManager.ins.Floor;

        MapProbabilityTable table = MapDB.LoadProbabilityTable(theme, floor);

        MT19937 mt = new MT19937();
        for(int i=0; i < 5;)
        {
            Vector2Int pos = new Vector2Int(
                mt.RandomRange(0, model.Size.x - 1),
                mt.RandomRange(0, model.Size.y - 1));

            if (_visit.Contains(pos)) continue;
            if (!model.isPlaceGroundEntity(pos)) continue;
            _visit.Add(pos);
            MapEntityID id = table.Rand(layer);
            MapBrain.ins.Create(layer, pos, id);
            i++;
        }
    }

    public void SpawnPlayer(MapModel model)
    {
        List<Vector2Int> poses = model.StartPos;      
        MT19937 mt = new MT19937();
        int idx = mt.RandomRange(0, poses.Count - 1);

        _visit.Add(poses[idx]);
        MapBrain.ins.Create(MapEntityLayer.GroundEntity, poses[idx], MapEntityID.Player);
    }
}
