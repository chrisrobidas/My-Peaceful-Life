using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [HideInInspector] public PlayerInput PlayerInput;

    public bool IsCurrentDeviceMouse
    {
        get
        {
            return PlayerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    public bool IsCurrentDeviceGamepad
    {
        get
        {
            return PlayerInput.currentControlScheme == "Gamepad";
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        PlayerInput = GetComponent<PlayerInput>();
    }
}
