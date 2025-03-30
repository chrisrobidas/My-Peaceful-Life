using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private InventoryButton[] _inventoryButtons;

    private void OnEnable()
    {
        RefreshInventoryButtons();
    }

    private void RefreshInventoryButtons()
    {
        Inventory localPlayerInventory = PlayersManager.Instance.GetLocalPlayerInventory();

        for (int i = 0; i < _inventoryButtons.Length; i++)
        {
            Item item = localPlayerInventory.Items[i];

            if (item != Item.EmptyItem)
            {
                int itemIndex = i;
                _inventoryButtons[i].Button.interactable = true;
                _inventoryButtons[i].Button.onClick.AddListener(() => DropItemFromInventory(itemIndex));
                _inventoryButtons[i].Image.sprite = item.ItemData.Sprite;
                _inventoryButtons[i].Image.color = Color.white;
                _inventoryButtons[i].AmountImage.SetActive(true);
                _inventoryButtons[i].AmountText.text = item.StackAmount + "";
            }
            else
            {
                ClearInventoryButton(i);
            }
        }
    }

    private void ClearInventoryButton(int buttonIndex)
    {
        _inventoryButtons[buttonIndex].Button.interactable = false;
        _inventoryButtons[buttonIndex].Button.onClick.RemoveAllListeners();
        _inventoryButtons[buttonIndex].Image.sprite = null;
        _inventoryButtons[buttonIndex].Image.color = Color.clear;
        _inventoryButtons[buttonIndex].AmountImage.SetActive(false);
        _inventoryButtons[buttonIndex].AmountText.text = 0 + "";
    }

    private void DropItemFromInventory(int itemIndex)
    {
        Item removedItem = PlayersManager.Instance.GetLocalPlayerInventory().RemoveItem(itemIndex);
        PlayersManager.Instance.GetLocalPlayerItemDropper().DropItem(removedItem);
        ClearInventoryButton(itemIndex);
    }
}
