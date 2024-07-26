using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        SetCursorLockMode();
    }

    private void SetCursorLockMode()
    {
        if (InputManager.Instance.IsCurrentDeviceGamepad)
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
}
