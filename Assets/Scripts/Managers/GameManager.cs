using Fusion;
using Fusion.Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus
{
    Playing,
    Inventory,
    PauseMenu,
    ToolSelection,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameStatus PreviousGameStatus { get; private set; }
    public GameStatus CurrentGameStatus { get; private set; }

    [Header("Start Game Setup")]
    public NetworkRunner RunnerPrefab;
    public Action OnChangeCurrentGameStatus;

    private NetworkRunner _networkRunner;
    private AuthenticationValues _photonAuthenticationValues;

    public IEnumerator StartGame()
    {
        yield return StartCoroutine(WaitForPhotonAuthenticationValues());

        _networkRunner = Instantiate(RunnerPrefab);

        SceneRef gameScene = SceneRef.FromIndex(Constants.GAME_SCENE_INDEX);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(gameScene);

        _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            PlayerCount = 10,
            SessionName = "MyPeacefulLifeRoom",
            Scene = gameScene,
            AuthValues = _photonAuthenticationValues,
        });
    }

    public void StopGame()
    {
        _networkRunner.Shutdown();
        _networkRunner = null;
    }

    private IEnumerator WaitForPhotonAuthenticationValues()
    {
        while (_photonAuthenticationValues == null)
        {
            Debug.Log("Waiting for the Photon authentication values...");
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetPhotonAuthenticationValues(AuthenticationValues photonAuthenticationValues)
    {
        _photonAuthenticationValues = photonAuthenticationValues;
    }

    public void SetCurrentGameStatus(GameStatus gameStatus)
    {
        if (gameStatus == CurrentGameStatus) return;

        PreviousGameStatus = CurrentGameStatus;
        CurrentGameStatus = gameStatus;

        if (OnChangeCurrentGameStatus != null)
        {
            OnChangeCurrentGameStatus.Invoke();
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
                SetCurrentGameStatus(GameStatus.PauseMenu);
                break;
            case Constants.GAME_SCENE_INDEX:
                if (_networkRunner == null) StartCoroutine(StartGame());
                SetCurrentGameStatus(GameStatus.Playing);
                break;
        }
    }
}
