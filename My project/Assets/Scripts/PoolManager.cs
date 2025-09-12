using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private List<GameObject>[] pools;

    private void Awake()
    {
        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
        // list √ ±‚»≠
    }

    private GameObject Get(int index)
    {
        GameObject select = null;
        foreach(var item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);

        }

        return select;

    }


}
