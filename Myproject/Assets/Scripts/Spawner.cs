using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public MonsterPool pool;
    public Transform player;

    public float normalSpawnInterval = 2f;
    private float normalTimer;

    public float eliteSpawnInterval = 90f;
    private float eliteTimer;

    private float gameTimer = 0f;

    private float rushStart = 180f;
    private float rushInterval = 180f;
    private int rushCount = 0;
    public PoolManager poolManager;

    public float bossSpawnTime = 600f;
    private bool bossSpawned = false;
    public GameObject bossPrefab;    

    private void Update()
    {
        float delta = Time.deltaTime;
        gameTimer += delta;
        normalTimer += delta;
        eliteTimer += delta;

        if (normalTimer >= normalSpawnInterval)
        {
            normalTimer = 0f;
            SpawnRandomNormal();
        }

        if (eliteTimer >= eliteSpawnInterval)
        {
            eliteTimer = 0f;
            SpawnRandomElite();
        }

        if (gameTimer >= rushStart + rushInterval * rushCount)
        {
            rushCount++;
            StartCoroutine(MonsterRush());
        }

        if (!bossSpawned && gameTimer >= bossSpawnTime)
        {
            bossSpawned = true;
            SpawnRandomBoss();
        }
    }

    private IEnumerator MonsterRush()
    {
        int rushCount = 200; 
        float rushDelay = 0.05f; 

        for (int i = 0; i < rushCount; i++)
        {
            SpawnRandomNormal();
            yield return new WaitForSeconds(rushDelay);
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

        var nonBossElites = elites.FindAll(m => !m.isBoss);
        if (nonBossElites.Count == 0)
        {
            return;
        }

        BossMonsterData data = nonBossElites[Random.Range(0, nonBossElites.Count)];
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 10f;
        pool.Get(data.id, spawnPos);
    }

    private void SpawnRandomBoss()
    {
        var bosses = pool.bossMonsters;
        if (bosses.Count == 0)
        {
            return;
        }

        BossMonsterData data = bosses[Random.Range(0, bosses.Count)];
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 10f;

        GameObject bossObj = pool.Get(data.id, spawnPos);
        if (bossObj != null)
        {
            BossMonster boss = bossObj.GetComponent<BossMonster>();
            if (boss != null)
            {
                boss.Init(data, player.GetComponent<Player>(), poolManager);
            }
        }
    }



}
