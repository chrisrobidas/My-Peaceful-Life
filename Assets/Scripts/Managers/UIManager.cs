using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private ToolsWheel _toolsWheel;
    [SerializeField] private Image _selectedToolImage;

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

    public void CallOpenToolsWheel()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _toolsWheel.OpenToolsWheel();
        GameManager.Instance.SetIsSelectingTool(true);
    }

    public void CallCloseToolsWheel()
    {
        _toolsWheel.CloseToolsWheel();
        GameManager.Instance.SetIsSelectingTool(false);
    }

    public void UpdateSelectedTool(int selectedToolID, Sprite icon)
    {
        _toolsWheel.UpdateSelectedToolID(selectedToolID);
        _selectedToolImage.sprite = icon;
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
