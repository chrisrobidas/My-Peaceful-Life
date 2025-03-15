using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public struct GameplayInput
{
    public Vector2 LookRotation;
    public Vector2 MoveDirection;
    public bool IsJumping;
    public bool IsSprinting;
}

public struct ToolSelectionInput
{
    public Vector2 Selection;
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputs : MonoBehaviour
{
    public Action OnInteract;
    public GameplayInput CurrentGameplayInput => _gameplayInput;
    public ToolSelectionInput CurrentToolSelectionInput => _toolSelectionInput;

    private GameplayInput _gameplayInput;
    private ToolSelectionInput _toolSelectionInput;
    private PlayerInput _playerInput;

    public bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    public bool IsCurrentDeviceGamepad
    {
        get
        {
            return _playerInput.currentControlScheme == "Gamepad";
        }
    }


    // PLAYER ACTIONS
    public void OnMoveInput(CallbackContext context)
    {
        _gameplayInput.MoveDirection = context.ReadValue<Vector2>();
    }

    public void OnLookInput(CallbackContext context)
    {
        _gameplayInput.LookRotation = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(CallbackContext context)
    {
        if (context.started)
        {
            _gameplayInput.IsJumping = true;
        }
        else if (context.canceled)
        {
            _gameplayInput.IsJumping = false;
        }
    }

    public void OnSprintInput(CallbackContext context)
    {
        if (context.started)
        {
            _gameplayInput.IsSprinting = true;
        }
        else if (context.canceled)
        {
            _gameplayInput.IsSprinting = false;
        }
    }

    public void OnInteractInput(CallbackContext context)
    {
        if (context.started)
        {
            OnInteract.Invoke();
        }
    }

    public void OnShowInventoryMenuInput(CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.ShowInventoryMenuPanel();
        }
    }

    public void OnShowPauseMenuInput(CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.ShowPauseMenuPanel();
        }
    }

    public void OnShowToolsWheelInput(CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.CallOpenToolsWheel();
        }
    }

    // INVENTORY MENU ACTIONS
    public void OnHideInventoryMenuInput(CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.HideInventoryMenuPanel();
        }
    }

    // PAUSE MENU ACTIONS
    public void OnHidePauseMenuInput(CallbackContext context)
    {
        if (context.started)
        {
            UIManager.Instance.HidePauseMenuPanel();
        }
    }

    // TOOL SELECTION ACTIONS
    public void OnHideToolsWheelInput(CallbackContext context)
    {
        if (context.canceled)
        {
            UIManager.Instance.CallCloseToolsWheel();
        }
    }

    public void OnSelectionInput(CallbackContext context)
    {
        _toolSelectionInput.Selection = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnChangeCurrentGameStatus += SetCurrentActionMap;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnChangeCurrentGameStatus -= SetCurrentActionMap;
    }

    private void Update()
    {
        SetCursorLockMode();
    }

    private void SetCursorLockMode()
    {
        if (IsCurrentDeviceGamepad)
        {
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        if (GameManager.Instance.CurrentGameStatus == GameStatus.Playing)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void SetCurrentActionMap()
    {
        switch (GameManager.Instance.CurrentGameStatus)
        {
            case GameStatus.Inventory:
                _playerInput.SwitchCurrentActionMap(Constants.INVENTORY_MENU_ACTION_MAP_NAME);
                break;
            case GameStatus.PauseMenu:
                _playerInput.SwitchCurrentActionMap(Constants.PAUSE_MENU_ACTION_MAP_NAME);
                break;
            case GameStatus.ToolSelection:
                _playerInput.SwitchCurrentActionMap(Constants.TOOL_SELECTION_ACTION_MAP_NAME);
                break;
            default:
                _playerInput.SwitchCurrentActionMap(Constants.PLAYER_ACTION_MAP_NAME);
                break;
        }
    }
}
