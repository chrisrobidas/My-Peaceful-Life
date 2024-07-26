using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public void OnMoveInput(CallbackContext context)
		{
            SetMoveInput(context.ReadValue<Vector2>());
		}

		public void OnLookInput(CallbackContext context)
		{
			if(cursorInputForLook)
			{
				SetLookInput(context.ReadValue<Vector2>());
			}
		}

		public void OnJumpInput(CallbackContext context)
		{
            if (context.started)
            {
                SetJumpInput(true);
            }
            else if (context.canceled)
            {
                SetJumpInput(false);
            }
        }

		public void OnSprintInput(CallbackContext context)
		{
            if (context.started)
            {
                SetSprintInput(true);
            }
            else if (context.canceled)
            {
                SetSprintInput(false);
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

        public void SetMoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void SetLookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void SetJumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SetSprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
	}
	
}