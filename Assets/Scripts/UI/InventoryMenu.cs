using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private InventoryButton[] _inventoryButtons;

    private void OnEnable()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        Inventory localPlayerInventory = PlayersManager.Instance.GetLocalPlayerInventory();

        for (int i = 0; i < _inventoryButtons.Length; i++)
        {
            Item item = localPlayerInventory.Items[i];

            if (item.Name != Item.DEFAULT_ITEM_NAME)
            {
                _inventoryButtons[i].Sprite = item.Sprite;
                _inventoryButtons[i].Count.text = item.StackAmount + "";
                _inventoryButtons[i].Button.interactable = true;
                _inventoryButtons[i].Button.onClick.AddListener(() => DropItemFromInventory(i));
            }
            else
            {
                _inventoryButtons[i].Sprite = null;
                _inventoryButtons[i].Count.text = 0 + "";
                _inventoryButtons[i].Button.interactable = false;
                _inventoryButtons[i].Button.onClick.RemoveAllListeners();
            }
        }
    }

    private void DropItemFromInventory(int itemIndex)
    {
        Item removedItem = PlayersManager.Instance.GetLocalPlayerInventory().RemoveItem(itemIndex);
        PlayersManager.Instance.GetLocalPlayerItemDropper().DropItem(removedItem);
    }
}
