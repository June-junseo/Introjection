using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    private Dictionary<int, List<GameObject>> pools = new();

    private void Awake()
    {
        foreach (var prefab in prefabs)
        {
            int id = prefab.GetComponent<SkillPrefabID>().id;
            pools[id] = new List<GameObject>();
        }
    }

    public GameObject Get(int id)
    {
        if (!pools.ContainsKey(id))
        {
            Debug.LogWarning($"Pool에 존재하지 않는 id: {id}");
            return null;
        }

        foreach (var obj in pools[id])
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject prefab = null;
        foreach (var p in prefabs)
        {
            if (p.GetComponent<SkillPrefabID>().id == id)
            {
                prefab = p;
                break;
            }
        }

        if (!prefab)
        {
            return null;
        }

        GameObject instance = Instantiate(prefab, transform);
        pools[id].Add(instance);
        instance.SetActive(true);
        return instance;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
    }
}
