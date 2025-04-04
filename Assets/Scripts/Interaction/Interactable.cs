using UnityEngine;

public enum InteractionAnimation
{
    PickUp,
    Push,
    Give
}

public abstract class Interactable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_LAYER))
        {
            PlayerInputs localPlayerPlayerInputs = PlayersManager.Instance.GetLocalPlayerPlayerInputs();
            if (localPlayerPlayerInputs != null && localPlayerPlayerInputs.OnInteract == null)
            {
                PlayersManager.Instance.GetLocalPlayerPlayerInputs().OnInteract += Interact;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_LAYER))
        {
            PlayerInputs localPlayerPlayerInputs = PlayersManager.Instance.GetLocalPlayerPlayerInputs();
            if (localPlayerPlayerInputs != null && localPlayerPlayerInputs.OnInteract != null)
            {
                PlayersManager.Instance.GetLocalPlayerPlayerInputs().OnInteract -= Interact;
            }
        }
    }

    // TODO: ONLY ALLOW TO INTERACT WITH 1 INTERACTABLE AT ONCE
    protected void Interact()
    {
        InteractAction();
    }

    protected abstract void InteractAction();
}
