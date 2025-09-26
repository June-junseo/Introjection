using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectSpawner : MonoBehaviour
{
    public PoolManager poolManager;       
    public int breakablePrefabId;       
    public int spawnCount = 10;           
    public Vector2 mapMin;                
    public Vector2 mapMax;                
    public Player player;                 

    private List<GameObject> spawnedObjects = new();

    private void Start()
    {
        SpawnBreakables();
    }

    public void SpawnBreakables()
    {
        for (int i = 0; i < spawnCount; i++)
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
