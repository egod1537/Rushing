using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Threading.Tasks;
using System.Diagnostics;
using MersenneTwister;
using UnityEngine.Events;
using System.Text;
using DG.Tweening;

public enum MapType
{
    Boss,
    Next,
    Puzzle,
    Monster,
    Shop,
    Secret,
    Road
}
public enum MapObjectType
{
    None,
    Damagable,
    Block,
    NextFloor,
    Portal,
    Treasure
}
public enum MapEntityType
{
    None,
    Player,
    Monster,
    Item,
    Object
}
public enum Direction
{
    Down,
    Right,
    Up,
    Left
}
public class MapManager : Singletone<MapManager>
{
    [Serializable]
    public struct MapGenerateResult
    {
        [SerializeField]
        public float time, progress;
        [SerializeField]
        public Vector2Int startPos;
        [SerializeField]
        public string path;
    }

    const int MAX_ITERATION_COUNT = 100;

    public class GenerateMapEvent : UnityEvent { }
    public GenerateMapEvent OnGenerateMap = new GenerateMapEvent();

    public class GenerateEntityEvent : UnityEvent { }
    public GenerateEntityEvent OnGenerateEntity = new GenerateEntityEvent();

    public UnityEvent onGenerateSuccessed = new UnityEvent();

    public int size
    {
        get { return resourceTable.n; }
        set { resourceTable.n = value; }
    }
    public int treasure;
    [Serializable]
    public class MapLayoutDictionary : SerializeDictionary<Vector2Int, MapObjectType> { }
    [Serializable]  
    public class MapObjectDictionary : SerializeDictionary<Vector2Int, GameObject> { }
    [Serializable]
    public class MapEntityDictionary : SerializeDictionary<Vector2Int, MapEntity> { }
    [Serializable]
    public class MapItemDictionary : SerializeDictionary<Vector2Int, MapItem> { }

    [SerializeField]
    public MapLayoutDictionary map = new MapLayoutDictionary();
    [SerializeField]
    public MapObjectDictionary mapObjects = new MapObjectDictionary();
    [SerializeField]
    public MapEntityDictionary mapEntity = new MapEntityDictionary();
    [SerializeField]
    public MapItemDictionary mapItem = new MapItemDictionary();

    [SerializeField]
    public SOMapTheme mapTheme;
    [SerializeField]
    public SOMapMonster mapMonster;
    [SerializeField]
    public MapResourceTable resourceTable = new MapResourceTable();

    [SerializeField]
    public bool isGenerated;
    [SerializeField]
    public MapGenerateResult generatorRet = new MapGenerateResult();

    public MapEntity player;

    private MapController _controller;
    public MapController controller
    {
        get
        {
            if (_controller == null) _controller = GetComponent<MapController>();
            return _controller;
        }
    }

    private MapBrain _brain;
    public MapBrain brain
    {
        get
        {
            if(_brain == null) _brain = GetComponent<MapBrain>();
            return _brain;
        }
    }

    public MapObjectSummoner objectSummoner = new MapObjectSummoner();
    public MapEntitySummoner entitySummoner = new MapEntitySummoner();
    public MapItemSummoner itemSummoner = new MapItemSummoner();

    MapGenerator generator = new MapGenerator();

    public bool isClear;

    Queue<UnityAction> que = new Queue<UnityAction>();

    private void Awake()
    {
        GameManager.ins.OnGameClear.AddListener(() =>
        {
            ProcessPortal();
        });
    }

    private void Update()
    {
        while(que.Count > 0)
            que.Dequeue()();
    }

    public async void GenerateMap()
    {

        isGenerated = false;

        Stopwatch watch = new Stopwatch();
        watch.Start();

        RemoveMap();

        var task = Task.Run(() => { 
            var pair = generator.GenerateMap(
            MAX_ITERATION_COUNT, resourceTable, ref generatorRet.progress);

            generatorRet.startPos = pair.Item1;
            generatorRet.path = pair.Item2;
            map.Copy(pair.Item3.Seriailize());

            que.Enqueue(() => { 
                brain.ClearQueue();
                player.DOKill(); 
                IEnumerator dd()
                {
                    yield return new WaitForSeconds(0.25f);
                    controller.ResetPlayer();
                }
                StartCoroutine(dd());
            });
        });
        await task;

        MapObjectDictionary mod = new MapObjectDictionary();
        MapEntityDictionary med = new MapEntityDictionary();
        mod.Copy(generator.GenerateMapObject(resourceTable, mapTheme, map));
        med.Copy(generator.GenerateEntity(resourceTable, map, mapMonster));
        mapItem = new MapItemDictionary();

        treasure = 0;
        for(int i=0; i <  size; i++)
            for(int j=0; j < size; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                if (mod[pos] == null) continue;
                
                if (map[pos] == MapObjectType.Treasure)
                {
                    itemSummoner.Summon(pos, mod[pos]);
                    treasure++;
                }
                else
                    objectSummoner.Summon(pos, mod[pos]);
            }
        generatorRet.progress = 0.0f;
        isGenerated = true;

        watch.Stop();
        generatorRet.time = watch.ElapsedMilliseconds;

        OnGenerateMap.Invoke();

        GenerateEntity();
    }

    public void GenerateEntity()
    {
        controller.ResetPlayer();

        for(int i=0; i < 5; i++)
        {
            Vector2Int pos = new Vector2Int(0, 0);
            while(map[pos] == MapObjectType.Block || 
                mapEntity.ContainsKey(pos) ||
                pos == generatorRet.startPos)
                pos = new Vector2Int(UnityEngine.Random.RandomRange(0, size-1), UnityEngine.Random.RandomRange(0, size-1)) ;
            entitySummoner.Summon(pos, Resources.Load("Entity/Monster") as GameObject);
        }

        OnGenerateEntity.Invoke();
    }

    public void GeneratePortal()
    {
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                map.Add(new Vector2Int(i, j), MapObjectType.None);
        List<Vector2Int> poses = new List<Vector2Int>();
        poses.Add(new Vector2Int(0, size / 2));
        poses.Add(new Vector2Int(size-1, size / 2));
        poses.Add(new Vector2Int(size/2, 0));
        poses.Add(new Vector2Int(size / 2, size -1));
        foreach(Vector2Int pos in poses)
            map[pos] = MapObjectType.Portal;

        StringBuilder sb = new StringBuilder();
        for(int i=0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
                sb.Append(map[new Vector2Int(i, j)].ToString() + " ");
            sb.AppendLine();
        }
        UnityEngine.Debug.Log(sb.ToString());

        MapObjectDictionary mod = new MapObjectDictionary();
        mod.Copy(generator.GenerateMapObject(resourceTable, mapTheme, map));
        for(int i=0; i < size; i++)
            for(int j=0; j < size; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                if (map[pos] == MapObjectType.None) continue;
                objectSummoner.Summon(new Vector2Int(i, j), mod[pos]);
            }
    }

    public void RemoveMap()
    {
        isGenerated = false;

        objectSummoner.DestroyAll();
        entitySummoner.DestroyAll();
        itemSummoner.DestroyAll();

        map.Clear();
        mapObjects.Clear();
        mapItem.Clear();
    }

    public void ProcessPortal()
    {
        RemoveMap();
        brain.ClearQueue();
        GeneratePortal();
        controller.TeleportEntity(player, size / 2, size / 2);
    }

    public void LoadMap()
    {

    }
}
