public class CollectibleItem : Interactable
{
    public Item Item;

    protected override void InteractAction()
    {
        PickUp();
    }

    private void PickUp()
    {
        PlayersManager.Instance.GetLocalPlayerThirdPersonController().PlayInteractionAnimation(InteractionAnimation.PickUp, 1.7f);
        PlayersManager.Instance.GetLocalPlayerPlayerInputs().OnInteract -= Interact;
        PlayersManager.Instance.GetLocalPlayerInventory().AddItem(Item);
        ItemFactory.Instance.RecycleItem(Item, gameObject);
    }
}
