using StarterAssets;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance { get; private set; }

    private GameObject _localPlayer;
    private Inventory _localPlayerInventory;
    private StarterAssetsInputs _localPlayerStarterAssetsInputs;
    private ThirdPersonController _localPlayerThirdPersonController;

    public void SetLocalPlayer(GameObject player)
    {
        _localPlayer = player;
    }

    public GameObject GetLocalPlayer()
    {
        return _localPlayer;
    }

    public Inventory GetLocalPlayerInventory()
    {
        if (_localPlayerInventory == null)
        {
            _localPlayerInventory = GetLocalPlayer().GetComponent<Inventory>();
        }

        return _localPlayerInventory;
    }

    public StarterAssetsInputs GetLocalPlayerStarterAssetsInputs()
    {
        if (_localPlayerStarterAssetsInputs == null)
        {
            _localPlayerStarterAssetsInputs = GetLocalPlayer().GetComponent<StarterAssetsInputs>();
        }

        return _localPlayerStarterAssetsInputs;
    }

    public ThirdPersonController GetLocalPlayerThirdPersonController()
    {
        if (_localPlayerThirdPersonController == null)
        {
            _localPlayerThirdPersonController = GetLocalPlayer().GetComponent<ThirdPersonController>();
        }

        return _localPlayerThirdPersonController;
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
