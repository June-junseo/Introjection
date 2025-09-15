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

    public GameObject Get(int skillId)
    {
        if (!pools.ContainsKey(skillId))
        {
            Debug.LogWarning($"Pool에 존재하지 않는 skillId: {skillId}");
            return null;
        }

        foreach (var obj in pools[skillId])
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
            if (p.GetComponent<SkillPrefabID>().id == skillId)
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
        pools[skillId].Add(instance);
        instance.SetActive(true);
        return instance;
    }
}
