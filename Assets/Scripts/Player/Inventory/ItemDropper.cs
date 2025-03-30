using System.Collections;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private Transform _dropPoint;
    [SerializeField] private float _dropDuration = 1.0f;

    public void DropItem(Item item)
    {
        Terrain closestTerrain = GetClosestTerrain();

        if (closestTerrain == null) return;

        Vector3 dropPositionTarget = GetRandomDropPosition(closestTerrain);
        Quaternion dropRotationTarget = TerrainUtil.GetRotationFromTerrainNormal(dropPositionTarget, closestTerrain) * Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        GameObject collectibleItem = ItemFactory.Instance.SpawnItem(item, closestTerrain.transform);
        StartCoroutine(ParabolicDrop(collectibleItem, dropPositionTarget, dropRotationTarget, _dropDuration));
    }

    private Vector3 GetRandomDropPosition(Terrain terrain)
    {
        Vector3 terrainPosition = terrain.transform.position;

        float x = Random.Range(-3f, 3f) + transform.position.x;
        float z = Random.Range(-3f, 3f) + transform.position.z;
        float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPosition.y;

        return new Vector3(x, y, z);
    }

    private IEnumerator ParabolicDrop(GameObject collectibleItem, Vector3 positionTarget, Quaternion rotationTarget, float duration)
    {
        Vector3 start = _dropPoint.position;
        Quaternion startRotation = collectibleItem.transform.rotation;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Lerp position along the parabolic arc
            Vector3 position = Vector3.Lerp(start, positionTarget, t);
            position.y += Mathf.Sin(t * Mathf.PI) * 2f;
            collectibleItem.transform.position = position;

            // Slerp rotation towards the target rotation
            collectibleItem.transform.rotation = Quaternion.Slerp(startRotation, rotationTarget, t);

            yield return null;
        }

        // Ensure final position and rotation are exactly at the target
        collectibleItem.transform.position = positionTarget;
        collectibleItem.transform.rotation = rotationTarget;
    }

    public Terrain GetClosestTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                return terrain;
            }
        }
        return null;
    }
}
