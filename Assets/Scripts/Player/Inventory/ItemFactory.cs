using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance { get; private set; }

    private Dictionary<ItemData, Queue<GameObject>> _itemPool = new Dictionary<ItemData, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public GameObject SpawnItem(Item item, Transform parentTerrainTransform)
    {
        return SpawnItem(item, Vector3.zero, Quaternion.identity, parentTerrainTransform);
    }

    public GameObject SpawnItem(Item item, Vector3 position, Quaternion rotation, Transform parentTerrainTransform)
    {
        if (!_itemPool.ContainsKey(item.ItemData))
            _itemPool[item.ItemData] = new Queue<GameObject>();

        GameObject itemInstance;

        // Reuse from pool if available
        if (_itemPool[item.ItemData].Count > 0)
        {
            Debug.Log("Reusing item " + item.ItemData + " from pool.");
            itemInstance = _itemPool[item.ItemData].Dequeue();
            itemInstance.transform.SetParent(parentTerrainTransform);
            itemInstance.transform.position = position;
            itemInstance.transform.rotation = rotation;
            itemInstance.SetActive(true);
        }
        else
        {
            // Instantiate a new one if pool is empty
            Debug.Log("Instantiate a new item " + item.ItemData + " because pool is empty.");
            itemInstance = Instantiate(item.ItemData.Prefab, position, rotation, parentTerrainTransform);
        }

        return itemInstance;
    }

    public void RecycleItem(Item item, GameObject itemInstance)
    {
        itemInstance.SetActive(false);
        _itemPool[item.ItemData].Enqueue(itemInstance);
    }
}
