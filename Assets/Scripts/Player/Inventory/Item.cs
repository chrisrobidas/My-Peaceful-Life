using System;

[Serializable]
public class Item
{
    public ItemData ItemData;
    public int StackAmount;

    private static Item _emptyItem;

    public Item(ItemData itemData, int stackAmount = 1)
    {
        ItemData = itemData;
        StackAmount = stackAmount;
    }

    public static Item EmptyItem
    {
        get
        {
            if (_emptyItem == null)
            {
                _emptyItem = new Item(ItemData.EmptyItemData);
            }
            return _emptyItem;
        }
    }
}
