using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectSpawner : MonoBehaviour
{
    public PoolManager poolManager;
    public int breakablePrefabId;
    public Vector2 mapMin;
    public Vector2 mapMax;
    public Player player;

    public float spawnInterval = 90f;
    public int minSpawnCount = 5;
    public int maxSpawnCount = 20;

    private List<GameObject> spawnedObjects = new();

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            int count = Random.Range(minSpawnCount, maxSpawnCount + 1);
            SpawnBreakables(count);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnBreakables(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = poolManager.Get(breakablePrefabId);
            obj.transform.position = new Vector3(
                Random.Range(mapMin.x, mapMax.x),
                Random.Range(mapMin.y, mapMax.y),
                0f
            );
            obj.transform.rotation = Quaternion.identity;

            BreakableObject breakable = obj.GetComponent<BreakableObject>();
            if (breakable != null)
            {
                breakable.SetPlayer(player);
            }

            spawnedObjects.Add(obj);
        }
    }

    public void ClearBreakables()
    {
        foreach (var obj in spawnedObjects)
        {
            poolManager.Release(obj);
        }
        spawnedObjects.Clear();
    }
}
