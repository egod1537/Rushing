using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExVector2
{
    public static Vector2 AddX(this Vector2 vec, float var)
    {
        vec.x += var;
        return vec;
    }
    public static Vector2 AddY(this Vector2 vec, float var)
    {
        vec.y += var;
        return vec;
    }
    public static Vector2 SetX(this Vector2 vec, float var)
    {
        vec.x = var;
        return vec;
    }
    public static Vector2 SetY(this Vector2 vec, float var)
    {
        vec.y = var;
        return vec;
    }

    public static Vector2 SetXY(this Vector2 vec, float a, float b)
        => vec.SetX(a).SetY(b);

    public static Vector2 AddXY(this Vector2 vec, float a, float b)
    => vec.AddX(a).AddY(b);
}
