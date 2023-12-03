using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapBrain : MonoBehaviour
{
    private Queue<UnityAction> brainQueue = new Queue<UnityAction>();
    private void Awake()
    {
        MapManager.ins.OnGenerateMap.AddListener(() =>
        {
            ClearQueue();
        });
    }
    private void Update()
    {
        while(brainQueue.Count > 0) brainQueue.Dequeue()();
    }

    public void AddQueue(UnityAction action)
        => brainQueue.Enqueue(action);
    public void ClearQueue()
        => brainQueue.Clear();
}
