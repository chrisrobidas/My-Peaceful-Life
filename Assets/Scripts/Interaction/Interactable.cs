using System.Collections.Generic;
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

    private static HashSet<Interactable> _interactablesInRange = new HashSet<Interactable>();
    private static Interactable _currentlyInteractable;
    private static Transform _playerTransform;

    private void Start()
    {
        _interactableControlUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_LAYER))
        {
            _playerTransform = other.transform;
            _interactablesInRange.Add(this);
            UpdateCurrentInteractable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_LAYER))
        {
            RemoveFromInteractables();
            UpdateCurrentInteractable();
        }
    }

    private void OnDisable()
    {
        RemoveFromInteractables();
        UpdateCurrentInteractable();
    }

    private void RemoveFromInteractables()
    {
        _interactablesInRange.Remove(this);
        if (_currentlyInteractable == this)
            RemoveInteractCallback();
    }

    private void UpdateCurrentInteractable()
    {
        if (_playerTransform == null || _interactablesInRange.Count == 0)
        {
            if (_currentlyInteractable != null)
                _currentlyInteractable.RemoveInteractCallback();

            _currentlyInteractable = null;
            return;
        }

        // Find closest interactable
        Interactable closest = null;
        float closestDist = float.MaxValue;

        foreach (var interactable in _interactablesInRange)
        {
            float dist = Vector3.Distance(_playerTransform.position, interactable.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = interactable;
            }
        }

        // Make the closest interactable the only one that can be interacted with
        if (_currentlyInteractable != closest)
        {
            if (_currentlyInteractable != null)
                _currentlyInteractable.RemoveInteractCallback();

            _currentlyInteractable = closest;
            _currentlyInteractable.AddInteractCallback();
        }
    }

    private void AddInteractCallback()
    {
        var inputs = PlayersManager.Instance.GetLocalPlayerPlayerInputs();
        if (inputs != null)
        {
            inputs.OnInteract += Interact;
            _interactableControlUI.SetActive(true);
        }
    }

    private void RemoveInteractCallback()
    {
        var inputs = PlayersManager.Instance.GetLocalPlayerPlayerInputs();
        if (inputs != null)
        {
            inputs.OnInteract -= Interact;
            _interactableControlUI.SetActive(false);
        }
    }

    protected void Interact()
    {
        if (_playerTransform.parent.GetComponent<ThirdPersonController>().IsInteracting) return;
        InteractAction();
    }

    protected abstract void InteractAction();
}
