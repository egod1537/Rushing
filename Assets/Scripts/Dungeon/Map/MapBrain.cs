using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(MapController))]
public class MapBrain : Singletone<MapBrain>
{
    private const int BRAIN_CLOCK_MS = 250;

    public class TurnData
    {
        public MapTurnType turnType;
        public EntityController caster;
        public Vector2Int targetPosition;
        public ISkill skill;
        public MapEntityID id;
        public MapEntityLayer layer;
        public UnityAction<bool> callback;

        public TurnData(MapTurnType turnType)
        {
            this.turnType = turnType;
        }
        public TurnData SetLayer(MapEntityLayer layer)
        {
            this.layer= layer;
            return this;
        }
        public TurnData SetID(MapEntityID id)
        {
            this.id = id;
            return this;
        }
        public TurnData SetCaster(EntityController caster)
        {
            this.caster = caster;
            return this;
        }
        public TurnData SetTargetPosition(Vector2Int targetPosition)
        {
            this.targetPosition = targetPosition;
            return this;
        }
        public TurnData SetSkill(ISkill skill)
        {
            this.skill = skill;
            return this;
        }
        public TurnData SetCallback(UnityAction<bool> callback)
        {
            this.callback = callback;
            return this;
        }
    }

    [SerializeField]
    public List<Entity> entityList = new List<Entity>();
    
    public Queue<TurnData> processQueue = new Queue<TurnData>();

    private MapController _controller;
    public MapController controller { get => _controller ??= GetComponent<MapController>(); }

    private MapModel _model;
    public MapModel model { get => _model ??= GetComponent<MapModel>(); }

    public void Start()
    {
        StartCoroutine(BrainLoop());
    }

    private readonly WaitForSeconds wfs = new WaitForSeconds(BRAIN_CLOCK_MS/1000.0f);
    IEnumerator BrainLoop()
    {
        if (Process() > 0)
            yield return wfs;
        else
            yield return null;
        
        StartCoroutine(BrainLoop());
    }

    private void ProcessDeath()
    {
        List<Entity> tempList = new List<Entity>();
        foreach (Entity e in entityList)
        {
            if (e != null && !e.state.HasFlag(EntityState.Dead)) tempList.Add(e);
        }
        entityList = new List<Entity>(tempList);
    }
    public int Process()
    {
        ProcessDeath();
        int sz = processQueue.Count;
        for(int i=0; i < sz; i++)
        {
            TurnData top = processQueue.Dequeue();
            if (isNeedCastType(top.turnType) && top.caster == null) continue;
            if (top.caster != null && top.caster.entity.state.HasFlag(EntityState.Dead)) continue;

            bool ret = false;
            switch (top.turnType)
            {
                case MapTurnType.Move:
                    ret = controller.Move(top.caster, top.targetPosition);
                    break;
                case MapTurnType.Attack:
                    ret = controller.Attack(top.caster, top.targetPosition);
                    break;
                case MapTurnType.Skill:
                    ret = controller.Skill(top.caster, top.targetPosition, top.skill);
                    break;
                case MapTurnType.Create:
                    ret = controller.Create(top.layer, top.targetPosition, top.id);
                    break;
                case MapTurnType.Death:
                    ret = controller.Death(top.caster);
                    break;
            }
            top.callback?.Invoke(ret);
        }
        ProcessDeath();

        return sz;
    }

    private bool isNeedCastType(MapTurnType type)
        => type == MapTurnType.Move || type == MapTurnType.Attack || type == MapTurnType.Skill;

    public void Move(EntityController caster, Vector2Int to, UnityAction<bool> callback=null)
        => processQueue.Enqueue(new TurnData(MapTurnType.Move)
            .SetCaster(caster)
            .SetTargetPosition(to)
            .SetCallback(callback));
    public void Attack(EntityController caster, Vector2Int to, UnityAction<bool> callback = null)
        => processQueue.Enqueue(new TurnData(MapTurnType.Attack)
            .SetCaster(caster)
            .SetTargetPosition(to)
            .SetCallback(callback));
    public void Skill(EntityController caster, Vector2Int to, ISkill skill, UnityAction<bool> callback = null)
        => processQueue.Enqueue(new TurnData(MapTurnType.Skill)
            .SetCaster(caster)
            .SetTargetPosition(to)
            .SetSkill(skill)
            .SetCallback(callback));
    public void Create(MapEntityLayer layer, Vector2Int to, MapEntityID id, UnityAction<bool> callback = null)
        => processQueue.Enqueue(new TurnData(MapTurnType.Create)
            .SetLayer(layer)
            .SetTargetPosition(to)
            .SetID(id)
            .SetCallback(callback));
    public void Create(MapEntityLayer layer, int x, int y, MapEntityID id, UnityAction<bool> callback = null)
        => Create(layer, new Vector2Int(x, y), id);
    public void Death(EntityController caster, UnityAction<bool> callback = null)
        => processQueue.Enqueue(new TurnData(MapTurnType.Death)
            .SetCaster(caster)
            .SetCallback(callback));
}
