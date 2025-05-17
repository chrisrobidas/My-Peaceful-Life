using UnityEngine;

public enum InteractionAnimation
{
    PickUp,
    Push,
    Give
}

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject _interactableControlUI;

    private void Start()
    {
        _interactableControlUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_LAYER))
        {
            PlayerInputs localPlayerPlayerInputs = PlayersManager.Instance.GetLocalPlayerPlayerInputs();
            if (localPlayerPlayerInputs != null && localPlayerPlayerInputs.OnInteract == null)
            {
                PlayersManager.Instance.GetLocalPlayerPlayerInputs().OnInteract += Interact;
                _interactableControlUI.SetActive(true);
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
                _interactableControlUI.SetActive(false);
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
