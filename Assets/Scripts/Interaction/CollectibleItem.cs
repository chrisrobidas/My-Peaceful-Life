using Fusion;

public class CollectibleItem : Interactable, IStateAuthorityChanged
{
    public Item Item;

    private bool _shouldDespawn;

    protected override void InteractAction()
    {
        PickUp();
    }

    private void PickUp()
    {
        PlayersManager.Instance.GetLocalPlayerThirdPersonController().PlayInteractionAnimation(InteractionAnimation.PickUp, 1.7f);
        PlayersManager.Instance.GetLocalPlayerPlayerInputs().OnInteract -= Interact;
        PlayersManager.Instance.GetLocalPlayerInventory().AddItem(Item);

        if (HasStateAuthority)
        {
            _shouldDespawn = true;
        }
        else
        {
            Object.RequestStateAuthority();
        }
    }

    public void StateAuthorityChanged()
    {
        _shouldDespawn = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (_shouldDespawn)
        {
            _shouldDespawn = false;
            Runner.Despawn(Object);
        }
    }
}
