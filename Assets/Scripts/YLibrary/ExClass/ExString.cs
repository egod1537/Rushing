using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExString 
{
    public static string Reverse(this string str)
    {
        char[] temp = str.ToCharArray();
        Array.Reverse(temp);
        return new string(temp);
    }
}
