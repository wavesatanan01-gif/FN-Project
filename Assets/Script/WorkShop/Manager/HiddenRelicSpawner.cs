using UnityEngine;

public class HiddenRelicSpawner : MonoBehaviour
{
    [Header("Time Condition")]
    public bool useTimeCondition = false;
    public float timeToSpawn = 120f;

    private float elapsedTime = 0f;
    [Header("Relic ที่จะ spawn")]
    public GameObject relicPrefab;

    [Header("จุดสุ่ม spawn relic ที่ซ่อน")]
    public Transform[] spawnPoints;

    [Header("เงื่อนไขปล่อย relic")]
    public int killsNeeded = 10;
    public int maxRelicsToSpawn = 3;

    private int currentKillCount = 0;
    private int relicSpawnedCount = 0;

    private void OnEnable()
    {
        Enemy.OnAnyEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        Enemy.OnAnyEnemyKilled -= HandleEnemyKilled;
    }

    private void HandleEnemyKilled(Enemy enemy)
    {
        if (relicSpawnedCount >= maxRelicsToSpawn)
            return;

        if (useTimeCondition) return; 

        currentKillCount++;
        Debug.Log($"[RELIC] Kill count = {currentKillCount}/{killsNeeded}");

        if (currentKillCount >= killsNeeded)
        {
            SpawnHiddenRelic();
            currentKillCount = 0;
        }
    }

    private void SpawnHiddenRelic()
    {
        if (relicPrefab == null)
        {
            Debug.LogWarning("[RELIC] relicPrefab ยังไม่ถูกเซ็ตใน Inspector");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[RELIC] spawnPoints array ว่าง");
            return;
        }

        Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(relicPrefab, p.position, p.rotation);
        relicSpawnedCount++;

        Debug.Log($"[RELIC] Spawned hidden relic #{relicSpawnedCount} at {p.name}");
    }
    private void Update()
    {
        if (useTimeCondition && relicSpawnedCount < maxRelicsToSpawn)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= timeToSpawn)
            {
                SpawnHiddenRelic();
                elapsedTime = 0f;
                Debug.Log($"[RELIC] Spawned by TIME after {timeToSpawn} sec");
            }
        }
    }

}
