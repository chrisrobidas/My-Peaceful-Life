using Fusion;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private CollectibleItem _collectibleItem;
    [SerializeField] private int _itemCount;

    private Terrain _terrain;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _terrain = GetComponent<Terrain>();
            SpawnItems();
        }
    }

    private void SpawnItems()
    {
        for (int i = 0; i < _itemCount; i++)
        {
            Vector3 spawnPosition = TerrainUtil.GetRandomPositionOnTerrain(_terrain);
            Quaternion spawnRotation = TerrainUtil.GetRotationFromTerrainNormal(spawnPosition, _terrain) * Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            NetworkObject spawnedItem = Runner.Spawn(_collectibleItem.Item.ItemData.Prefab, spawnPosition, spawnRotation);
            spawnedItem.transform.SetParent(gameObject.transform);
        }
    }
}
