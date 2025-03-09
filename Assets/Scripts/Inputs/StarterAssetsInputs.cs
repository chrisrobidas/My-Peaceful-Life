using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace StarterAssets
{
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
    public class StarterAssetsInputs : MonoBehaviour
    {
        public Action OnInteract;

        public GameplayInput CurrentGameplayInput => _gameplayInput;
        private GameplayInput _gameplayInput;

        public ToolSelectionInput CurrentToolSelectionInput => _toolSelectionInput;
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

        public void OnPauseInput(CallbackContext context)
        {
            if (context.started)
            {
                UIManager.Instance.ShowPauseMenuPanel();
            }
        }

        public void OnOpenToolsWheelInput(CallbackContext context)
        {
            if (context.started)
            {
                UIManager.Instance.CallOpenToolsWheel();
            }
        }

        public void OnInteractInput(CallbackContext context)
        {
            if (context.started)
            {
                OnInteract.Invoke();
            }
        }

        // UI ACTIONS
        public void OnResumeInput(CallbackContext context)
        {
            if (context.started)
            {
                UIManager.Instance.HidePauseMenuPanel();
            }
        }

        // TOOL SELECTION ACTIONS
        public void OnCloseToolsWheelInput(CallbackContext context)
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
            GameManager.Instance.OnChangeIsGamePaused += SetCurrentActionMap;
            GameManager.Instance.OnChangeIsSelectingTool += SetCurrentActionMap;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnChangeIsGamePaused -= SetCurrentActionMap;
            GameManager.Instance.OnChangeIsSelectingTool -= SetCurrentActionMap;
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

            if (GameManager.Instance.IsGamePaused || GameManager.Instance.IsSelectingTool)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void SetCurrentActionMap()
        {
            if (GameManager.Instance.IsGamePaused)
            {
                _playerInput.SwitchCurrentActionMap(Constants.UI_ACTION_MAP_NAME);
            }
            else if (GameManager.Instance.IsSelectingTool)
            {
                _playerInput.SwitchCurrentActionMap(Constants.TOOL_SELECTION_ACTION_MAP_NAME);
            }
            else
            {
                _playerInput.SwitchCurrentActionMap(Constants.PLAYER_ACTION_MAP_NAME);
            }
        }
    }
}
