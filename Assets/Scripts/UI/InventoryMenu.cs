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
                _inventoryButtons[i].Button.interactable = true;
                _inventoryButtons[i].Button.onClick.AddListener(() => DropItemFromInventory(i));
                _inventoryButtons[i].Image.sprite = item.Sprite;
                _inventoryButtons[i].Image.color = Color.white;
                _inventoryButtons[i].AmountImage.SetActive(true);
                _inventoryButtons[i].AmountText.text = item.StackAmount + "";
            }
            else
            {
                _inventoryButtons[i].Button.interactable = false;
                _inventoryButtons[i].Button.onClick.RemoveAllListeners();
                _inventoryButtons[i].Image.sprite = null;
                _inventoryButtons[i].Image.color = Color.clear;
                _inventoryButtons[i].AmountImage.SetActive(false);
                _inventoryButtons[i].AmountText.text = 0 + "";
            }
        }
    }

    private void DropItemFromInventory(int itemIndex)
    {
        Item removedItem = PlayersManager.Instance.GetLocalPlayerInventory().RemoveItem(itemIndex);
        PlayersManager.Instance.GetLocalPlayerItemDropper().DropItem(removedItem);
    }
}
