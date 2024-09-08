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

    [RequireComponent(typeof(PlayerInput))]
    public class StarterAssetsInputs : MonoBehaviour
    {
        public GameplayInput CurrentInput => _input;
        private GameplayInput _input;

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

        public void OnMoveInput(CallbackContext context)
		{
            _input.MoveDirection = context.ReadValue<Vector2>();
		}

		public void OnLookInput(CallbackContext context)
		{
            _input.LookRotation = context.ReadValue<Vector2>();
        }

		public void OnJumpInput(CallbackContext context)
		{
            if (context.started)
            {
                _input.IsJumping = true;
            }
            else if (context.canceled)
            {
                _input.IsJumping = false;
            }
        }

		public void OnSprintInput(CallbackContext context)
		{
            if (context.started)
            {
                _input.IsSprinting = true;
            }
            else if (context.canceled)
            {
                _input.IsSprinting = false;
            }
        }

        public void OnPauseInput(CallbackContext context)
        {
            if (context.started)
            {
				UIManager.Instance.ShowPauseMenuPanel();
            }
        }

        public void OnResumeInput(CallbackContext context)
        {
            if (context.started)
            {
                UIManager.Instance.HidePauseMenuPanel();
            }
        }

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnChangeIsGamePaused += SetCurrentActionMap;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnChangeIsGamePaused -= SetCurrentActionMap;
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

            if (GameManager.Instance.IsGamePaused)
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
            else
            {
                _playerInput.SwitchCurrentActionMap(Constants.PLAYER_ACTION_MAP_NAME);
            }
        }
    }
}