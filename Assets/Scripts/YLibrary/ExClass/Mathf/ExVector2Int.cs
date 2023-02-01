using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExVector2Int
{
    public static Vector2Int AddX(this Vector2Int vec, int var)
    {
        vec.x += var;
        return vec;
    }
    public static Vector2Int AddY(this Vector2Int vec, int var)
    {
        vec.y += var;
        return vec;
    }
    public static Vector2Int SetX(this Vector2Int vec, int var)
    {
        vec.x = var;
        return vec;
    }
    public static Vector2Int SetY(this Vector2Int vec, int var)
    {
        vec.y = var;
        return vec;
    }

    public static Vector2Int SetXY(this Vector2Int vec, int a, int b)
        => vec.SetX(a).SetY(b);

    public static Vector2Int AddXY(this Vector2Int vec, int a, int b)
    => vec.AddX(a).AddY(b);
}
