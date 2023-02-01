using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}
public static class MapTool
{
    public static readonly int[] dx = { 0, -1, 1, 0, 0 };
    public static readonly int[] dy = { 0, 0, 0, -1, 1 };

    public static readonly Dictionary<Direction, Vector2Int> dxy =
    new Dictionary<Direction, Vector2Int> {
        { Direction.None, Vector2Int.zero},
        { Direction.Up, Vector2Int.left},
        { Direction.Down, Vector2Int.right},
        { Direction.Left, Vector2Int.down},
        { Direction.Right, Vector2Int.up}
    };

    public static Direction Vector2Direction(Vector2Int vec)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        for (int i = 0; i < 4; i++) list.Add(new Vector2Int(dx[i], dy[i]));

        Direction ans = Direction.None;
        foreach(Direction d in Enum.GetValues(typeof(Direction)))
        {
            if ((dxy[ans] - vec).sqrMagnitude > (dxy[d] - vec).sqrMagnitude)
                ans = d;
        }
        return ans;
    }
}
