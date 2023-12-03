using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerModel))]
public class PlayerController : MonoBehaviour
{
    PlayerModel _model;
    PlayerModel model
    {
        get
        {
            if(_model == null) _model = GetComponent<PlayerModel>();
            return _model;
        }
    }
    private void Start()
    {
        var cs = ControllerSystem.ins;
        cs.OnLeftSlide.AddListener(() => { MapManager.ins.controller.RushEntity(model, Direction.Left); });
        cs.OnRightSlide.AddListener(() => { MapManager.ins.controller.RushEntity(model, Direction.Right); });
        cs.OnUpSlide.AddListener(() => { MapManager.ins.controller.RushEntity(model, Direction.Up); });
        cs.OnDownSlide.AddListener(() => { MapManager.ins.controller.RushEntity(model, Direction.Down); });
    }
}
