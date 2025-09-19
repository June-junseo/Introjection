using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public MonsterDatabase database;
    public Rigidbody2D playerTarget;
    public PoolManager poolManager;

    private Dictionary<MonsterType, Queue<GameObject>> poolDict = new();

    private void Awake()
    {
        foreach (var data in database.monsters)
        {
            Queue<GameObject> queue = new Queue<GameObject>();

            for (int i = 0; i < data.poolSize; i++)
            {
                GameObject obj = Instantiate(data.prefab);
                obj.SetActive(false);

                Monster monster = obj.GetComponent<Monster>();
                monster.Init(data, this, playerTarget, poolManager);

                queue.Enqueue(obj);
            }

            poolDict[data.type] = queue;
        }
    }

    public GameObject Get(MonsterType type, Vector3 position)
    {
        if (!poolDict.ContainsKey(type))
        {
            Debug.LogError($"MonsterPool: {type} Ç® ¾øÀ½!");
            return null;
        }

        var queue = poolDict[type];
        GameObject obj = queue.Count > 0 ? queue.Dequeue() : Instantiate(database.GetData(type).prefab);

        obj.transform.position = position;
        obj.SetActive(true);

        obj.GetComponent<Monster>().Init(database.GetData(type), this, playerTarget, poolManager);

        return obj;
    }

    public void Return(MonsterType type, GameObject obj)
    {
        obj.SetActive(false);
        poolDict[type].Enqueue(obj);
    }
}
