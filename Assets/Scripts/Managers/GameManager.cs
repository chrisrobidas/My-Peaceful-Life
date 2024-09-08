using Fusion;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Start Game Setup")]
    public NetworkRunner RunnerPrefab;

    public bool IsGamePaused { get; private set; }

    public Action OnChangeIsGamePaused;

    private NetworkRunner _networkRunner;

    public async void StartGame()
    {
        _networkRunner = Instantiate(RunnerPrefab);

        SceneRef gameScene = SceneRef.FromIndex(Constants.GAME_SCENE_INDEX);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(gameScene);

        await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            PlayerCount = 10,
            SessionName = "MyPeacefulLifeRoom",
            Scene = gameScene,
        });
    }

    public async void StopGame()
    {
        await _networkRunner.Shutdown();
        _networkRunner = null;
    }

    public void SetIsGamePaused(bool isGamePaused)
    {
        if (isGamePaused == IsGamePaused) return;

        IsGamePaused = isGamePaused;

        if (OnChangeIsGamePaused != null)
        {
            OnChangeIsGamePaused.Invoke();
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
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeActiveScene();
    }

    private void InitializeActiveScene()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case Constants.MAIN_MENU_SCENE_INDEX:
                SetIsGamePaused(true);
                break;
            case Constants.GAME_SCENE_INDEX:
                if (_networkRunner == null) StartGame();
                SetIsGamePaused(false);
                break;
        }
    }
}
