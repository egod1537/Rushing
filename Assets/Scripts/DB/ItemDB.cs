using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemCode
{
    Treasure
}
public class ItemDB : MonoBehaviour
{
    const string ICON_PATH = "DB/Item/Icons";
    const string DATA_PATH = "DB/Item/Data";

    public static Texture2D GetItemIcon(ItemCode code)
        => Resources.Load($"{ICON_PATH}/{code.ToString()}") as Texture2D;
}
