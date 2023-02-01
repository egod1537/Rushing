using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDB : MonoBehaviour
{
    private static readonly string DB_PATH = "Map";
    private static readonly string TABLE_DB_PATH = $"{DB_PATH}/Table";

    public static MapProbabilityTable LoadProbabilityTable(MapTheme theme, int floor)
    {
        return Resources.Load($"{TABLE_DB_PATH}/{(int)theme + 1}.{theme}/{floor}")
            as MapProbabilityTable;
    }
}
