using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private bool _isPlayerNear;

    private void OnEnable()
    {
        PlayersManager.Instance.GetLocalPlayerStarterAssetsInputs().OnInteract += Interact;
    }

    private void OnDisable()
    {
        PlayersManager.Instance.GetLocalPlayerStarterAssetsInputs().OnInteract -= Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_LAYER))
        {
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_LAYER))
        {
            _isPlayerNear = false;
        }
    }

    // TODO: ONLY ALLOW TO INTERACT WITH 1 INTERACTABLE AT ONCE
    private void Interact()
    {
        if (_isPlayerNear)
        {
            InteractAction();
        }
    }

    protected abstract void InteractAction();
}
