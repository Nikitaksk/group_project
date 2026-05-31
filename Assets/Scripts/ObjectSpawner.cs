using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] fallingObjectPrefabs;
    public float spawnRate = 1.5f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 0f, spawnRate);
    }

    void SpawnObject()
    {
        if (fallingObjectPrefabs == null || fallingObjectPrefabs.Length == 0)
        {
            Debug.LogWarning("ObjectSpawner: No prefabs assigned.");
            return;
        }

        GameObject prefab = fallingObjectPrefabs[Random.Range(0, fallingObjectPrefabs.Length)];
        Bounds spawnBounds = spriteRenderer.bounds;
        float randomX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        Vector3 spawnPosition = new Vector3(randomX, spawnBounds.min.y, transform.position.z);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
