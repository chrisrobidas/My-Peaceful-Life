using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemImage;
    [SerializeField] private string _maxAmount;

    private int _amount; // mmmm??

    private void PickUp()
    {
        PlayersManager.Instance.GetLocalPlayerInventory().AddItem(this);
    }

    private void Drop()
    {
        PlayersManager.Instance.GetLocalPlayerInventory().RemoveItem(this);
    }
}
