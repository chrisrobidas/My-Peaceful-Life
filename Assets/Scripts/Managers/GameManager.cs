using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGamePaused { get; private set; }

    public void SetIsGamePaused(bool isGamePaused)
    {
        if (isGamePaused == IsGamePaused) return;

        IsGamePaused = isGamePaused;

        if (IsGamePaused)
        {
            InputManager.Instance.PlayerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            InputManager.Instance.PlayerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        InitializeActiveScene();
    }

    private void InitializeActiveScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case Constants.MAIN_MENU_SCENE_NAME:
                SetIsGamePaused(true);
                break;
            case Constants.GAME_SCENE_NAME:
                SetIsGamePaused(false);
                break;
        }
    }
}
