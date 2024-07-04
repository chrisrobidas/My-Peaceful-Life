using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenuPanel;

    public void OnResumeInput(CallbackContext context)
    {
        if (context.started)
        {
            HidePauseMenuPanel();
        }
    }

    public void OnBackInput(CallbackContext context)
    {
        if (context.started)
        {
            HidePauseMenuPanel();
        }
    }

    public void ShowPauseMenuPanel()
    {
        _pauseMenuPanel.SetActive(true);
        GameManager.Instance.SetIsGamePaused(true);
    }

    public void HidePauseMenuPanel()
    {
        _pauseMenuPanel.SetActive(false);
        GameManager.Instance.SetIsGamePaused(false);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
}
