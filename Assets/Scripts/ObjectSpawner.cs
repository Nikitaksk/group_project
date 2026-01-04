using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectSpawner : MonoBehaviour
{
    // Use an array to hold the different trash prefabs
    public GameObject[] fallingObjectPrefabs;
    public float spawnRate = 1.5f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Optional: Disable the renderer so the spawner is invisible during the game
        // spriteRenderer.enabled = false;
    }

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, spawnRate);
    }

    void SpawnObject()
    {
        // Check if the prefabs array is not null and has elements
        if (fallingObjectPrefabs != null && fallingObjectPrefabs.Length > 0)
        {
            // Select a random prefab from the array
            int randomIndex = Random.Range(0, fallingObjectPrefabs.Length);
            GameObject selectedPrefab = fallingObjectPrefabs[randomIndex];

            Bounds spawnBounds = spriteRenderer.bounds;
            float randomX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
            Vector3 spawnPosition = new Vector3(randomX, spawnBounds.min.y, transform.position.z);

            // Instantiate the randomly selected prefab
            Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Falling Object Prefabs array is not assigned or is empty in the Spawner!");
        }
    }
}
