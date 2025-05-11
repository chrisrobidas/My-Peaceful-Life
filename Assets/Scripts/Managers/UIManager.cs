using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _playerMenusPanel;
    [SerializeField] private GameObject _gameplayPanel;

    [SerializeField] private ToolsWheel _toolsWheel;
    [SerializeField] private GameObject _selectToolControlImage;
    [SerializeField] private Image _selectedToolImage;
    [SerializeField] private GameObject _selectedToolImageBackground;

    public void ShowPauseMenuPanel()
    {
        _pauseMenuPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
        _selectedToolImageBackground.SetActive(false);
        GameManager.Instance.SetCurrentGameStatus(GameStatus.PauseMenu);
    }

    public void HidePauseMenuPanel()
    {
        _pauseMenuPanel.SetActive(false);
        _gameplayPanel.SetActive(true);
        _selectedToolImageBackground.SetActive(true);
        GameManager.Instance.SetCurrentGameStatus(GameStatus.Playing);
    }

    public void ShowInventoryMenuPanel()
    {
        _playerMenusPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
        _selectedToolImageBackground.SetActive(false);
        GameManager.Instance.SetCurrentGameStatus(GameStatus.Inventory);
    }

    public void HideInventoryMenuPanel()
    {
        _playerMenusPanel.SetActive(false);
        _gameplayPanel.SetActive(true);
        _selectedToolImageBackground.SetActive(true);
        GameManager.Instance.SetCurrentGameStatus(GameStatus.Playing);
    }

    public void CallOpenToolsWheel()
    {
        GameManager.Instance.SetCurrentGameStatus(GameStatus.ToolSelection);
        _gameplayPanel.SetActive(false);
        _selectToolControlImage.SetActive(true);
        _toolsWheel.OpenToolsWheel();
    }

    public void CallCloseToolsWheel()
    {
        GameManager.Instance.SetCurrentGameStatus(GameStatus.Playing);
        _gameplayPanel.SetActive(true);
        _selectToolControlImage.SetActive(false);
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
