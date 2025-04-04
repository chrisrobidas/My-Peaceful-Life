using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private CollectibleItem _collectibleItem;
    [SerializeField] private int _itemCount;

    private Terrain _terrain;

    private void Start()
    {
        _terrain = GetComponent<Terrain>();
        SpawnItems();
    }

    private void SpawnItems()
    {
        for (int i = 0; i < _itemCount; i++)
        {
            Vector3 spawnPosition = TerrainUtil.GetRandomPositionOnTerrain(_terrain);
            Quaternion spawnRotation = TerrainUtil.GetRotationFromTerrainNormal(spawnPosition, _terrain) * Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            ItemFactory.Instance.SpawnItem(_collectibleItem.Item, spawnPosition, spawnRotation, gameObject.transform);
        }
    }
}
