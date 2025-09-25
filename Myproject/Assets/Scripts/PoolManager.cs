using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; 
    private Dictionary<int, List<GameObject>> pools = new();

    private void Awake()
    {
        foreach (var prefab in prefabs)
        {

            SkillPrefabID skillIdComp = prefab.GetComponent<SkillPrefabID>();
            if (skillIdComp != null)
            {
                foreach (var id in skillIdComp.ids)
                {
                    if (!pools.ContainsKey(id)) pools[id] = new List<GameObject>();
                }
            }

            itemPrefabId itemIdComp = prefab.GetComponent<itemPrefabId>();
            if (itemIdComp != null)
            {
                int id = itemIdComp.id;
                if (!pools.ContainsKey(id))
                {
                    pools[id] = new List<GameObject>();
                }

                }
            }
    }

    public GameObject Get(int id)
    {
        if (!pools.ContainsKey(id))
        {
            Debug.LogWarning($"PoolManager: 존재하지 않는 ID {id}");
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
            SkillPrefabID skillIdComp = p.GetComponent<SkillPrefabID>();
            if (skillIdComp != null && System.Array.Exists(skillIdComp.ids, x => x == id))
            {
                prefab = p;
                break;
            }

            itemPrefabId itemIdComp = p.GetComponent<itemPrefabId>();
            if (itemIdComp != null && itemIdComp.id == id)
            {
                prefab = p;
                break;
            }
        }

        if (!prefab)
        {
            Debug.LogWarning($"PoolManager: Prefab 존재하지 않음 ID {id}");
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
