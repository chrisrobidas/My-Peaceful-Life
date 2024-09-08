using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    public override void Spawned()
    {
        int spawnIndex = Random.Range(0, _spawnPoints.Length);
        var player = Runner.Spawn(_playerPrefab, _spawnPoints[spawnIndex].position, Quaternion.identity, Runner.LocalPlayer);

        // Enables inputs for the local player
        player.gameObject.GetComponent<StarterAssetsInputs>().enabled = true;
        player.gameObject.GetComponent<PlayerInput>().enabled = true;
    }
}
