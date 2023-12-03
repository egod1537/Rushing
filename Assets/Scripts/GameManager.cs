using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singletone<GameManager>
{
    public class GameClearEvent : UnityEvent { }
    public GameClearEvent OnGameClear = new GameClearEvent();

    MapManager map { get { return MapManager.ins; } }

    void Start()
    {
        Application.targetFrameRate = 60;

        OnGameClear.AddListener(() => {
            Debug.Log("Game Clear");
        });
    }

    private void Update()
    {
        if(!map.isClear && map.isGenerated && map.treasure == 0)
        {
            OnGameClear.Invoke();
        }
    }
}
