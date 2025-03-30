using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int INVENTORY_SIZE = 42;

    [HideInInspector] public List<Item> Items { get; private set; } = new List<Item>();

    private void Start()
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            Items.Add(Item.EmptyItem);
        }
    }

    public void AddItem(Item item)
    {
        Item existingSameItemWithStackNotToMax = Items.Find(i => i.ItemData.Name == item.ItemData.Name && i.StackAmount < i.ItemData.MaxStackAmount);
        int nextEmptyItemIndex = Items.FindIndex(i => i == Item.EmptyItem);

        if (existingSameItemWithStackNotToMax != null)
        {
            // Add as much as possible to the existing stack
            int availableSpace = existingSameItemWithStackNotToMax.ItemData.MaxStackAmount - existingSameItemWithStackNotToMax.StackAmount;
            int amountToAdd = Mathf.Min(availableSpace, item.StackAmount);
            existingSameItemWithStackNotToMax.StackAmount += amountToAdd;
            Debug.Log("Added to existing stack " + amountToAdd + " " + item.ItemData.Name + " items to the player inventory.");

            // If there's overflow, create a new item entry
            int remainingAmount = item.StackAmount - amountToAdd;
            if (remainingAmount > 0)
            {
                Debug.Log("Added overflow " + remainingAmount + " " + item.ItemData.Name + " items to the player inventory.");
                Items[nextEmptyItemIndex] = new Item(item.ItemData, remainingAmount);
            }
        }
        else
        {
            Debug.Log("Added new stack of " + item.StackAmount + " " + item.ItemData.Name + " items to the player inventory.");
            Items[nextEmptyItemIndex] = new Item(item.ItemData, item.StackAmount);
        }

        Debug.Log("Added " + item.StackAmount + " " + item.ItemData.Name + " items to the player inventory.");
        PrintInventory();
    }

    public Item RemoveItem(int itemIndex)
    {
        Item itemToRemove = Items[itemIndex];
        Items[itemIndex] = Item.EmptyItem;
        Debug.Log("Removed " + itemToRemove.StackAmount + " " + itemToRemove.ItemData.Name + " items from the player inventory.");
        PrintInventory();
        return itemToRemove;
    }

    // Debug method
    public void PrintInventory()
    {
        string inventoryDisplay = "Current Inventory:\n";

        for (int i = 0; i < Items.Count; i++)
        {
            Item item = Items[i];

            if (item != Item.EmptyItem)
            {
                inventoryDisplay += $"Slot {i + 1}: {item.ItemData.Name} x{item.StackAmount}\n";
            }
            else
            {
                inventoryDisplay += $"Slot {i + 1}: [Empty]\n";
            }
        }

        Debug.Log(inventoryDisplay);
    }
}
