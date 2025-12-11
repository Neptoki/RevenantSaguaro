using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 20f;

    [Header("Scaling Settings")]
    public float startingHealth = 50f;
    public float healthIncrease = 10f;

    private float timer = 0f;
    private float nextHealth;

    void Start()
    {
        timer = spawnInterval;
        nextHealth = startingHealth;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Enemy prefab or spawn point not set!");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        Target target = enemy.GetComponent<Target>();
        if (target != null)
        {
            target.health = nextHealth;
        }

        nextHealth += healthIncrease;

        Debug.Log($"Spawned enemy with {target.health} health.");
    }
}