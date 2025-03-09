using System;
using UnityEngine;

[Serializable]
public class Item
{
    public const string DEFAULT_ITEM_NAME = "DefaultItem";

    public string Name;
    public int StackAmount;
    public int MaxStackAmount = 1;
    public Sprite Sprite;
    public GameObject Prefab;

    public Item()
    {
        Name = DEFAULT_ITEM_NAME;
    }

    public Item(string name, int stackAmount, int maxStackAmount, Sprite sprite, GameObject prefab)
    {
        Name = name;
        StackAmount = stackAmount;
        MaxStackAmount = maxStackAmount;
        Sprite = sprite;
        Prefab = prefab;
    }
}
