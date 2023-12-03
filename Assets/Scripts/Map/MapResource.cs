using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapResource : MonoBehaviour
{
    const string PREFIX_PATH = "Map/";
    private static string Path(string theme, MapType mapType, int n, int num)
        => $"{PREFIX_PATH}{theme}/{n}x{n}/{mapType}_{num}";
    public static SOMapTheme LoadTheme(string theme, MapType mapType, int n, int num)
        => Resources.Load(Path(theme, mapType, n, num)) as SOMapTheme;
}
