using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExVector3
{
    public static Vector3 AddX(this Vector3 vec, float var)
    {
        vec.x += var;
        return vec;
    }
    public static Vector3 AddY(this Vector3 vec, float var)
    {
        vec.y += var;
        return vec;
    }
    public static Vector3 AddZ(this Vector3 vec, float var)
    {
        vec.z += var;
        return vec;
    }
    public static Vector3 SetX(this Vector3 vec, float var)
    {
        vec.x = var;
        return vec;
    }
    public static Vector3 SetY(this Vector3 vec, float var)
    {
        vec.y = var;
        return vec;
    }
    public static Vector3 SetZ(this Vector3 vec, float var)
    {
        vec.z = var;
        return vec;
    }

    public static Vector3 SetXY(this Vector3 vec, float a, float b)
        => vec.SetX(a).SetY(b);
    public static Vector3 SetXZ(this Vector3 vec, float a, float b)
        => vec.SetX(a).SetZ(b);
    public static Vector3 SetYZ(this Vector3 vec, float a, float b)
        => vec.SetY(a).SetZ(b);

    public static Vector3 AddXY(this Vector3 vec, float a, float b)
    => vec.AddX(a).AddY(b);
    public static Vector3 AddXZ(this Vector3 vec, float a, float b)
        => vec.AddX(a).AddZ(b);
    public static Vector3 AddYZ(this Vector3 vec, float a, float b)
        => vec.AddY(a).AddZ(b);
}
