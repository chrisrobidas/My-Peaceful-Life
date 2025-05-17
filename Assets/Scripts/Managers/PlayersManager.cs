using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance { get; private set; }

    private GameObject _localPlayer;
    private Inventory _localPlayerInventory;
    private ItemDropper _localPlayerItemDropper;
    private PlayerInputs _localPlayerPlayerInputs;
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
            _localPlayerInventory = GetLocalPlayer()?.GetComponent<Inventory>();
        }

        return _localPlayerInventory;
    }

    public ItemDropper GetLocalPlayerItemDropper()
    {
        if (_localPlayerItemDropper == null)
        {
            _localPlayerItemDropper = GetLocalPlayer()?.GetComponent<ItemDropper>();
        }

        return _localPlayerItemDropper;
    }

    public PlayerInputs GetLocalPlayerPlayerInputs()
    {
        if (_localPlayerPlayerInputs == null)
        {
            _localPlayerPlayerInputs = GetLocalPlayer()?.GetComponent<PlayerInputs>();
        }

        return _localPlayerPlayerInputs;
    }

    public ThirdPersonController GetLocalPlayerThirdPersonController()
    {
        if (_localPlayerThirdPersonController == null)
        {
            _localPlayerThirdPersonController = GetLocalPlayer()?.GetComponent<ThirdPersonController>();
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
