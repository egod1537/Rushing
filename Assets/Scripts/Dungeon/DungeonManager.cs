using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DungeonManager : Singletone<DungeonManager>
{
    public MapTheme Theme;
    public int Floor;

    public Dictionary<Vector2Int, MapModel> Dungeon 
        = new Dictionary<Vector2Int, MapModel>();

    private MapProbabilityTable _table;
    public MapProbabilityTable table { get => _table ??= MapDB.LoadProbabilityTable(Theme, Floor); }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void Intiailize()
    {
        
    }
}
