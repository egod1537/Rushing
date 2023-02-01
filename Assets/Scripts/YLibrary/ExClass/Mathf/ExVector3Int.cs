using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExVector3Int
{
    public static Vector3Int AddX(this Vector3Int vec, int var)
    {
        vec.x += var;
        return vec;
    }
    public static Vector3Int AddY(this Vector3Int vec, int var)
    {
        vec.y += var;
        return vec;
    }
    public static Vector3Int AddZ(this Vector3Int vec, int var)
    {
        vec.z += var;
        return vec;
    }
    public static Vector3Int SetX(this Vector3Int vec, int var)
    {
        vec.x = var;
        return vec;
    }
    public static Vector3Int SetY(this Vector3Int vec, int var)
    {
        vec.y = var;
        return vec;
    }
    public static Vector3Int SetZ(this Vector3Int vec, int var)
    {
        vec.z = var;
        return vec;
    }

    public static Vector3Int SetXY(this Vector3Int vec, int a, int b)
        => vec.SetX(a).SetY(b);
    public static Vector3Int SetXZ(this Vector3Int vec, int a, int b)
        => vec.SetX(a).SetZ(b);
    public static Vector3Int SetYZ(this Vector3Int vec, int a, int b)
        => vec.SetY(a).SetZ(b);

    public static Vector3Int AddXY(this Vector3Int vec, int a, int b)
    => vec.AddX(a).AddY(b);
    public static Vector3Int AddXZ(this Vector3Int vec, int a, int b)
        => vec.AddX(a).AddZ(b);
    public static Vector3Int AddYZ(this Vector3Int vec, int a, int b)
        => vec.AddY(a).AddZ(b);
}
