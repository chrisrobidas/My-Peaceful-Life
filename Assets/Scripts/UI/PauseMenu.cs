using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _blackLeavePanel;

    public void Resume()
    {
        UIManager.Instance.HidePauseMenuPanel();
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuCoroutine());
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    private IEnumerator MainMenuCoroutine()
    {
        yield return LeaveGame();
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_INDEX);
    }

    private IEnumerator LeaveGame()
    {
        _blackLeavePanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.StopGame();
    }
}
