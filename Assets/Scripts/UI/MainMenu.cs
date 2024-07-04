using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(Constants.GAME_SCENE_NAME);
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
