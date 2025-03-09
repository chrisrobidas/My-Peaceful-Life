using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int INVENTORY_SIZE = 42;

    public List<Item> Items { get; private set; } = new List<Item>();

    private void Start()
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            Items.Add(new Item());
        }
    }

    public void AddItem(Item item)
    {
        Item existingSameItemWithStackNotToMax = Items.Find(i => i.Name == item.Name && i.StackAmount < i.MaxStackAmount);
        int nextEmptyItemIndex = Items.FindIndex(i => i.Name == Item.DEFAULT_ITEM_NAME);

        if (existingSameItemWithStackNotToMax != null)
        {
            // Add as much as possible to the existing stack
            int availableSpace = existingSameItemWithStackNotToMax.MaxStackAmount - existingSameItemWithStackNotToMax.StackAmount;
            int amountToAdd = Mathf.Min(availableSpace, item.StackAmount);
            existingSameItemWithStackNotToMax.StackAmount += amountToAdd;

            // If there's overflow, create a new item entry
            int remainingAmount = item.StackAmount - amountToAdd;
            if (remainingAmount > 0)
            {
                Items[nextEmptyItemIndex] = new Item(item.Name, remainingAmount, item.MaxStackAmount, item.Sprite, item.Prefab);
            }
        }
        else
        {
            Items[nextEmptyItemIndex] = new Item(item.Name, item.StackAmount, item.MaxStackAmount, item.Sprite, item.Prefab);
        }

        Debug.Log("Added " + item.StackAmount + " " + item.Name + " items to the player inventory.");
    }

    public Item RemoveItem(int itemIndex)
    {
        Item itemToRemove = Items[itemIndex];
        Items[itemIndex] = new Item();
        Debug.Log("Removed " + itemToRemove.StackAmount + " " + itemToRemove.Name + " items from the player inventory.");
        return itemToRemove;
    }
}
