using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBtnPlayerReset : MonoBehaviour
{
    public void Action()
    {
        MapManager.ins.controller.ResetPlayer();
    }
}
