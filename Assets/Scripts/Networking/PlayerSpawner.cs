using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerSpawner : NetworkBehaviour, IStateAuthorityChanged
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    [Networked]
    private int _spawnCount { get; set; }

    public override void Spawned()
    {
        int spawnIndex = Random.Range(0, _spawnPoints.Length);
        NetworkObject player = Runner.Spawn(_playerPrefab, _spawnPoints[spawnIndex].position, Quaternion.identity, Runner.LocalPlayer);

        // Enables inputs for the local player
        player.gameObject.GetComponent<StarterAssetsInputs>().enabled = true;
        player.gameObject.GetComponent<PlayerInput>().enabled = true;

        // Assigns a material based on how many player spawned so far
        ThirdPersonController thirdPersonController = player.gameObject.GetComponent<ThirdPersonController>();
        thirdPersonController.SetCharacterMaterialIndex(_spawnCount % thirdPersonController.PlayerPrefabMaterials.Length);

        IncrementSpawnCount();

        if (!HasStateAuthority)
        {
            Object.RequestStateAuthority();
        }
    }

    public void StateAuthorityChanged()
    {
        IncrementSpawnCount();
    }

    private void IncrementSpawnCount()
    {
        if (HasStateAuthority)
        {
            _spawnCount++;
        }
    }
}
