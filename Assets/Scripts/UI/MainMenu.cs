using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _loadingPanel;

    public void Play()
    {
        _mainMenuPanel.SetActive(false);
        _loadingPanel.SetActive(true);
        StartCoroutine(GameManager.Instance.StartGame());
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
