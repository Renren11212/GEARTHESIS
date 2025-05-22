using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject terrainPrefab;
    
    

    public void GenerateTerrain(Vector3 position)
    {
        GameObject terrain = Instantiate(terrainPrefab, position, Quaternion.identity);
        terrain.transform.SetParent(transform);
    }
}
