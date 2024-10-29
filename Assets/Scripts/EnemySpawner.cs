using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public Transform player;
    public Transform baseTarget;
    public Terrain terrain;
    public float spawnRadius = 10f;

    void Start()
    {
        Debug.Log("Player: " + player);
        Debug.Log("Terrain: " + terrain);
        Debug.Log("EnemyPrefab: " + enemyPrefab);

        InvokeRepeating("SpawnEnemy", 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);

        Vector3 spawnPosition = new Vector3(player.position.x + randomX, 0, player.position.z + randomZ);
        spawnPosition.x = Mathf.Clamp(spawnPosition.x, terrainPosition.x, terrainPosition.x + terrainSize.x);
        spawnPosition.z = Mathf.Clamp(spawnPosition.z, terrainPosition.z, terrainPosition.z + terrainSize.z);

        spawnPosition.y = terrain.SampleHeight(spawnPosition) + terrainPosition.y;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Rigidbody rb = newEnemy.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.MovePosition(spawnPosition);
        }
        else
        {
            newEnemy.transform.position = spawnPosition;
        }

        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetPlayer(player);
            enemyController.SetBase(baseTarget);
        }
    }
}
