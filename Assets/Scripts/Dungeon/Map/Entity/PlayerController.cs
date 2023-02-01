using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerController : EntityController
{
    public bool isRush = false;

    private void Update()
    {
        Vector2Int normal = new Vector2Int(
            -(int)Input.GetAxisRaw("Vertical"), (int)Input.GetAxisRaw("Horizontal"));

        if (normal == Vector2Int.zero) return;

        if(!isRush) Rush(MapTool.Vector2Direction(normal));
    }

    public void Rush(Direction dir)
    {
        isRush = true;

        int d = (int)dir;
        Vector2Int normal = new Vector2Int(MapTool.dx[d], MapTool.dy[d]);

        Move(entity.pos+normal, (bool ret) =>
        {
            if (!ret)
            {
                isRush = false;
                return;
            }
            Rush(dir);
        });
    }

    public void ResetPosition()
    {
        
    }
}
