using UnityEngine;

public class CollectibleItem : Interactable
{
    [SerializeField] private Item _item;

    protected override void InteractAction()
    {
        PickUp();
    }

    private void PickUp()
    {
        PlayersManager.Instance.GetLocalPlayerPlayerInputs().OnInteract -= Interact;
        PlayersManager.Instance.GetLocalPlayerInventory().AddItem(_item);
        Destroy(gameObject);
    }
}
