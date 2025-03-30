using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string Name;
    public int MaxStackAmount = 1;
    public Sprite Sprite;
    public GameObject Prefab;

    private const string EMPTY_ITEM_NAME = "EmptyItem";
    private static ItemData _emptyItemData;

    public static ItemData EmptyItemData
    {
        get
        {
            if (_emptyItemData == null)
            {
                _emptyItemData = CreateInstance<ItemData>();
                _emptyItemData.Name = EMPTY_ITEM_NAME;
            }
            return _emptyItemData;
        }
    }
}
