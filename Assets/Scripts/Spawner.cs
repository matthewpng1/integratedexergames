using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Array of object prefabs to spawn
    public float spawnInterval = 1f; // Time between spawns
    public float spawnPositionX = -10f; // X position to spawn from (left side, off-screen)
    public float minY = -4f; // Minimum Y position for spawn
    public float maxY = 4f; // Maximum Y position for spawn

    public GameObject currentObject; // Reference to the currently spawned object
    public bool gameRunning = true; // Flag to control if spawning is active

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start spawning objects at regular intervals
        InvokeRepeating("TrySpawnObject", 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TrySpawnObject()
    {
        // Only spawn if there is no current object on screen and game is running
        if (currentObject == null && gameRunning)
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        if (objectPrefabs.Length == 0) return; // No prefabs to spawn

        // Select a random prefab from the array
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        GameObject prefabToSpawn = objectPrefabs[randomIndex];

        // Generate a random Y position within the range
        float randomY = Random.Range(minY, maxY);

        // Instantiate the prefab at the spawn position
        currentObject = Instantiate(prefabToSpawn, new Vector3(spawnPositionX, randomY, 0f), Quaternion.identity);
    }
}
