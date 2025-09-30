using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    [Header("몬스터 종류")]
    public List<NormalMonsterData> normalMonsters;
    public List<BossMonsterData> eliteMonsters;
    public List<BossMonsterData> bossMonsters;

    public Rigidbody2D playerTarget;
    public PoolManager poolManager;

    private Dictionary<int, Queue<Monster>> normalPool = new();
    private Dictionary<int, Queue<Monster>> elitePool = new();
    private Dictionary<int, Queue<BossMonster>> bossPool = new();

    private void Awake()
    {
        // 일반 몬스터 풀 생성
        foreach (var data in normalMonsters)
        {
            if (data.prefab == null) continue;

            Queue<Monster> queue = new Queue<Monster>();
            for (int i = 0; i < 200; i++)
            {
                GameObject obj = Instantiate(data.prefab);
                obj.SetActive(false);

                Monster monster = obj.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.Init(data, this, playerTarget, poolManager);
                    queue.Enqueue(monster);
                }
            }
            normalPool[data.id] = queue;
        }

        // 엘리트 몬스터 풀 생성
        foreach (var data in eliteMonsters)
        {
            if (data.prefab == null) continue;

            Queue<Monster> queue = new Queue<Monster>();
            for (int i = 0; i < 3; i++)
            {
                GameObject obj = Instantiate(data.prefab);
                obj.SetActive(false);

                Monster monster = obj.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.Init(data, this, playerTarget, poolManager);
                    queue.Enqueue(monster);
                }
            }
            elitePool[data.id] = queue;
        }

        // 보스 몬스터 풀 생성
        foreach (var data in bossMonsters)
        {
            if (data.prefab == null) continue;

            Queue<BossMonster> queue = new Queue<BossMonster>();
            for (int i = 0; i < 1; i++)
            {
                GameObject obj = Instantiate(data.prefab);
                obj.SetActive(false);

                BossMonster boss = obj.GetComponent<BossMonster>();
                if (boss != null)
                {
                    boss.Init(data, playerTarget.GetComponent<Player>(), poolManager);
                    queue.Enqueue(boss);
                }
            }
            bossPool[data.id] = queue;
        }
    }

    #region Get
    public Monster GetNormal(int id, Vector3 position)
    {
        if (!normalPool.ContainsKey(id)) return null;

        var queue = normalPool[id];
        if (queue.Count == 0)
        {
            var data = normalMonsters.Find(m => m.id == id);
            GameObject obj = Instantiate(data.prefab);
            Monster monster = obj.GetComponent<Monster>();
            monster.Init(data, this, playerTarget, poolManager);
            queue.Enqueue(monster);
        }

        Monster m = queue.Dequeue();
        m.transform.position = position;
        m.gameObject.SetActive(true);
        return m;
    }

    public Monster GetElite(int id, Vector3 position)
    {
        if (!elitePool.ContainsKey(id)) return null;

        var queue = elitePool[id];
        if (queue.Count == 0)
        {
            var data = eliteMonsters.Find(m => m.id == id);
            GameObject obj = Instantiate(data.prefab);
            Monster monster = obj.GetComponent<Monster>();
            monster.Init(data, this, playerTarget, poolManager);
            queue.Enqueue(monster);
        }

        Monster m = queue.Dequeue();
        m.transform.position = position;
        m.gameObject.SetActive(true);
        return m;
    }

    public BossMonster GetBoss(int id, Vector3 position)
    {
        if (!bossPool.ContainsKey(id)) return null;

        var queue = bossPool[id];
        if (queue.Count == 0)
        {
            var data = bossMonsters.Find(m => m.id == id);
            GameObject obj = Instantiate(data.prefab);
            BossMonster boss = obj.GetComponent<BossMonster>();
            boss.Init(data, playerTarget.GetComponent<Player>(), poolManager);
            queue.Enqueue(boss);
        }

        BossMonster b = queue.Dequeue();
        b.transform.position = position;
        b.gameObject.SetActive(true);
        return b;
    }
    #endregion

    #region Return
    public void ReturnNormal(int id, Monster monster)
    {
        if (normalPool.ContainsKey(id))
        {
            monster.gameObject.SetActive(false);
            normalPool[id].Enqueue(monster);
        }
    }

    public void ReturnElite(int id, Monster monster)
    {
        if (elitePool.ContainsKey(id))
        {
            monster.gameObject.SetActive(false);
            elitePool[id].Enqueue(monster);
        }
    }

    public void ReturnBoss(int id, BossMonster boss)
    {
        if (bossPool.ContainsKey(id))
        {
            boss.gameObject.SetActive(false);
            bossPool[id].Enqueue(boss);
        }
    }
    #endregion
}
