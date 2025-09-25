using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public List<NormalMonsterData> normalMonsters;
    public List<EliteMonsterData> eliteMonsters;

    public Rigidbody2D playerTarget;
    public PoolManager poolManager;

    private Dictionary<int, Queue<GameObject>> poolDict = new();

    private void Awake()
    {
        foreach (var data in normalMonsters)
        {
            if (data.prefab == null)
            {
                continue;
            }

            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < 200; i++)
            {
                GameObject obj = Instantiate(data.prefab);
                obj.SetActive(false);

                Monster monster = obj.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.Init(data, this, playerTarget, poolManager);
                }

                queue.Enqueue(obj);
            }
            poolDict[data.id] = queue;
        }

        foreach (var data in eliteMonsters)
        {
            if (data.prefab == null)
            {
                continue;
            }

            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                GameObject obj = Instantiate(data.prefab);
                obj.SetActive(false);

                Monster monster = obj.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.Init(data, this, playerTarget, poolManager);
                }

                queue.Enqueue(obj);
            }
            poolDict[data.id] = queue;
        }
    }

    public GameObject Get(int id, Vector3 position)
    {
        if (!poolDict.ContainsKey(id))
        {
            Debug.LogWarning($"MonsterPool: {id} 풀 없음!");
            return null;
        }

        var queue = poolDict[id];
        GameObject obj;

        if (queue.Count > 0)
        {
            obj = queue.Dequeue();
        }
        else
        {
            Debug.Log($"MonsterPool: {id} 풀 부족 → 새 인스턴스 생성");

            var normalData = normalMonsters.Find(m => m.id == id);
            var eliteData = eliteMonsters.Find(m => m.id == id);

            GameObject prefab = normalData != null ? normalData.prefab : eliteData.prefab;
            obj = Instantiate(prefab);

            Monster monster = obj.GetComponent<Monster>();
            if (monster != null)
            {
                if (normalData != null)
                {
                    monster.Init(normalData, this, playerTarget, poolManager);
                }
                else if (eliteData != null)
                {
                    monster.Init(eliteData, this, playerTarget, poolManager);
                }
            }
        }

        obj.transform.position = position;

        obj.SetActive(true);
        return obj;
    }

    public void Return(int id, GameObject obj)
    {
        obj.SetActive(false);
        if (poolDict.ContainsKey(id))
        {
            poolDict[id].Enqueue(obj);
        }
    }
}
