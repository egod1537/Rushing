using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBackgroundLoader : MonoBehaviour
{
    public readonly string PATH_PREFIX = "Map/Background";
    public MapTheme Theme;
    public string BgPath;
    public string GetThemePath()
        => $"{PATH_PREFIX}/{(int)Theme + 1}.{Theme}";
    public string GetPath()
        => $"{GetThemePath()}/{BgPath}";
    public void LoadMapBackground()
    {
        ClearMapBackground();
        GameObject goBg = Instantiate(Resources.Load(GetPath()) as GameObject);
        goBg.transform.SetParent(transform);
    }
    public void ClearMapBackground()
    {
        int size = transform.childCount;
        for (int i = 0; i < size; i++)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }
}
