using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab ศัตรู (ต้องมี Enemy + EnemyScaling)")]
    public GameObject[] enemyPrefabs;

    [Header("จุดเกิดศัตรู")]
    public Transform[] spawnPoints;

    [Header("ตั้งค่าพื้นฐาน")]
    public float baseSpawnInterval = 5f;   
    public int baseCountPerWave = 2;       
    public int maxEnemyAlive = 15;        

    private float timer;

    private void Start()
    {
        timer = GetCurrentInterval();
    }

    private void Update()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        int currentEnemy = FindObjectsOfType<Enemy>().Length;

        if (currentEnemy == 0 && timer > 0.2f)
        {
            timer = 0.2f;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnWave();
            timer = GetCurrentInterval();
        }
    }


    private float GetSpawnMult()
    {
        if (DifficultyManager.I == null)
            return 1f;

        return DifficultyManager.I.SpawnMult();   
    }

    private float GetCurrentInterval()
    {
        float mult = GetSpawnMult();

        float interval = baseSpawnInterval / mult;

        return Mathf.Max(0.5f, interval);
    }

    private void SpawnWave()
    {
        int currentEnemy = FindObjectsOfType<Enemy>().Length;
       

        if (currentEnemy >= maxEnemyAlive)
            return;

        float mult = GetSpawnMult();

        int spawnCount = Mathf.RoundToInt(baseCountPerWave * mult);

        if (spawnCount < 1)
            spawnCount = 1;

        int canSpawn = maxEnemyAlive - currentEnemy;
        spawnCount = Mathf.Clamp(spawnCount, 0, canSpawn);

        if (spawnCount <= 0)
            return;

        for (int i = 0; i < spawnCount; i++)
        {
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogWarning("[Spawner] No spawn points assigned.");
                return;
            }

            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogWarning("[Spawner] No enemy prefabs assigned.");
                return;
            }

            Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (p == null)
            {
                Debug.LogWarning("[Spawner] Spawn point is null, skip.");
                continue;
            }

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            if (prefab == null)
            {
                Debug.LogWarning("[Spawner] Enemy prefab is null (maybe destroyed or Missing), skip.");
                continue;
            }

            GameObject enemy = Instantiate(prefab, p.position, p.rotation);
            Debug.Log($"[Spawner] Spawn {enemy.name} at {p.name} (#{i + 1}/{spawnCount})");
        }
    }
}



