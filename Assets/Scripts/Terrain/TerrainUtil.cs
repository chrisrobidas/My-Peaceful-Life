using UnityEngine;

public static class TerrainUtil
{
    public static Vector3 GetRandomPositionOnTerrain(Terrain terrain)
    {
        Vector3 terrainPos = terrain.transform.position;
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        float x = Random.Range(terrainPos.x, terrainPos.x + terrainWidth);
        float z = Random.Range(terrainPos.z, terrainPos.z + terrainLength);
        float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrainPos.y;

        return new Vector3(x, y, z);
    }

    public static Quaternion GetRotationFromTerrainNormal(Vector3 position, Terrain terrain)
    {
        Vector3 terrainPos = terrain.transform.position;
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        // Convert world position to normalized terrain coordinates (0 to 1)
        float normalX = (position.x - terrainPos.x) / terrainWidth;
        float normalZ = (position.z - terrainPos.z) / terrainLength;

        Vector3 terrainNormal = terrain.terrainData.GetInterpolatedNormal(normalX, normalZ);

        return Quaternion.FromToRotation(Vector3.up, terrainNormal);
    }
}
