using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singletone<PlayerManager>
{
    private PlayerModel _player;
    public PlayerModel player
    {
        get
        {
            if(_player == null)
                _player = MapManager.ins.player.GetComponent<PlayerModel>();
            return _player;
        }
    }
}
