using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        UIManager.Instance.HidePauseMenuPanel();
    }

    public void Restart()
    {
        SceneManager.LoadScene(Constants.GAME_SCENE_NAME);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_NAME);
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
