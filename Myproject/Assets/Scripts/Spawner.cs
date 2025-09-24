using UnityEngine;

public class Spawner : MonoBehaviour
{
    public MonsterPool pool;
    public MonsterDatabase database;
    public Transform player;

    [Header("Normal Monster")]
    public float normalSpawnInterval = 2f;
    private float normalTimer;

    [Header("Elite Monster")]
    public float eliteSpawnDelay = 90f; 
    public float eliteSpawnInterval = 90f; 
    private float eliteTimer;
    private bool eliteFirstSpawned = false;

    [Header("Boss Monster")]
    public float bossSpawnTime = 840f;
    private float bossTimer;
    private bool bossSpawned = false;

    private void OnEnable()
    {
        normalTimer = 0f;
        eliteTimer = 0f;
        eliteFirstSpawned = false;
        bossTimer = 0f;
        bossSpawned = false;
    }

    private void Update()
    {
        normalTimer += Time.deltaTime;
        if (normalTimer >= normalSpawnInterval)
        {
            normalTimer = 0f;
            SpawnRandomNormal();
        }

        eliteTimer += Time.deltaTime;
        if (!eliteFirstSpawned && eliteTimer >= eliteSpawnDelay)
        {
            eliteFirstSpawned = true;
            eliteTimer = 0f;
            SpawnRandomElite();
        }
        else if (eliteFirstSpawned && eliteTimer >= eliteSpawnInterval)
        {
            eliteTimer = 0f;
            SpawnRandomElite();
        }

        bossTimer += Time.deltaTime;
        if (!bossSpawned && bossTimer >= bossSpawnTime)
        {
            bossSpawned = true;
            SpawnRandomBoss();
        }
    }

    private void SpawnRandomNormal()
    {
        var normals = pool.normalMonsters;
        if (normals.Count == 0)
        {
            return;
        }

        NormalMonsterData data = normals[Random.Range(0, normals.Count)];
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 10f;
        pool.Get(data.id, spawnPos);
    }

    private void SpawnRandomElite()
    {
        var elites = pool.eliteMonsters;
        if (elites.Count == 0)
        {
            return;
        }

        EliteMonsterData data = elites[Random.Range(0, elites.Count)];
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 10f;
        pool.Get(data.id, spawnPos);
    }

    private void SpawnRandomBoss()
    {
        var bosses = pool.eliteMonsters;
        if (bosses.Count == 0)
        {
            return;
        }

        EliteMonsterData data = bosses[Random.Range(0, bosses.Count)];
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 10f;
        pool.Get(data.id, spawnPos);
    }
}
