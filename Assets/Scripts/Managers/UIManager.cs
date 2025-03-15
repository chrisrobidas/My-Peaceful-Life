using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _playerMenusPanel;
    [SerializeField] private ToolsWheel _toolsWheel;
    [SerializeField] private Image _selectedToolImage;

    public void ShowPauseMenuPanel()
    {
        _pauseMenuPanel.SetActive(true);
        GameManager.Instance.SetCurrentGameStatus(GameStatus.PauseMenu);
    }

    public void HidePauseMenuPanel()
    {
        _pauseMenuPanel.SetActive(false);
        GameManager.Instance.SetCurrentGameStatus(GameStatus.Playing);
    }

    public void ShowInventoryMenuPanel()
    {
        _playerMenusPanel.SetActive(true);
        GameManager.Instance.SetCurrentGameStatus(GameStatus.Inventory);
    }

    public void HideInventoryMenuPanel()
    {
        _playerMenusPanel.SetActive(false);
        GameManager.Instance.SetCurrentGameStatus(GameStatus.Playing);
    }

    public void CallOpenToolsWheel()
    {
        GameManager.Instance.SetCurrentGameStatus(GameStatus.ToolSelection);
        _toolsWheel.OpenToolsWheel();
    }

    public void CallCloseToolsWheel()
    {
        GameManager.Instance.SetCurrentGameStatus(GameStatus.Playing);
        _toolsWheel.CloseToolsWheel();
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
