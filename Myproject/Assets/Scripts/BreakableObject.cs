using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public int ID;
    public int stage = 1;

    public TextAsset csvFile;
    public GameObject hpItemPrefab;
    public GameObject goldItemPrefab;
    private Player player;

    private DropData dropInfo;
    private List<DropItem> activeDrops = new List<DropItem>();

    int droppedCount = 0;

    public void SetPlayer(Player p) => player = p;

    private void Awake()
    {
        if (csvFile == null)
        {
            Debug.LogWarning("CSV 파일 없음");
            return;
        }

        var drops = CSVLoader.LoadCSV<DropData>(csvFile);
        dropInfo = drops.Find(d => d.ID == ID && d.STAGE == stage);
    }

    private void OnEnable()
    {
        if (player != null)
        {
            StartCoroutine(DropRoutine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DropRoutine()
    {
        while (true)
        {
            DropItems(useObjectPosition: false);
            yield return new WaitForSeconds(90f);
        }
    }

    private void DropItems(bool useObjectPosition)
    {
        if (dropInfo == null || player == null)
        {
            return;
        }

        Vector2 dropCenter = useObjectPosition ? (Vector2)transform.position : (Vector2)player.transform.position;


        if (Random.value <= dropInfo.DROP1_RATE && goldItemPrefab != null)
        {
            int goldAmount = Random.Range(dropInfo.DROP1, dropInfo.DROP1_MaxCount + 1);
            Debug.Log($"[Drop] Gold Amount: {goldAmount} at {dropCenter}");
            SpawnDropItem(DropItem.ItemType.Gold, goldAmount,dropCenter);
        }

        if (Random.value <= dropInfo.DROP2_RATE && hpItemPrefab != null)
        {
            int hpAmount = Random.Range(dropInfo.DROP2_MinCount, dropInfo.DROP2_COUNT + 1);
            Debug.Log($"[Drop] HP Amount: {hpAmount} at {dropCenter}");
            SpawnDropItem(DropItem.ItemType.HP, hpAmount, dropCenter);
        }

        if (droppedCount == 0)
        {
            SpawnDropItem(DropItem.ItemType.HP, 1, dropCenter);
        }
    }

    private void SpawnDropItem(DropItem.ItemType type, int amount, Vector2 centerPos)
    {
        GameObject prefab = type == DropItem.ItemType.HP ? hpItemPrefab : goldItemPrefab;
        if (prefab == null)
        {
            return;
        }

        float radius = 0.5f;

        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPos;
            bool positionOk;
            int tries = 0;

            do
            {
                spawnPos = centerPos + Random.insideUnitCircle * Random.Range(0.5f, 1.5f);
                positionOk = true;

                foreach (var existing in activeDrops)
                {
                    if (Vector2.Distance(spawnPos, existing.transform.position) < radius)
                    {
                        positionOk = false;
                        break;
                    }
                }

                tries++;
                if (tries > 10)
                {
                    break;
                }
            }
            while (!positionOk);

            GameObject dropObj = Instantiate(prefab, spawnPos, Quaternion.identity);
            DropItem dropItem = dropObj.GetComponent<DropItem>();

           if (type == DropItem.ItemType.HP)
            {
                dropItem.hpAmount = 10;  
            }

            dropItem.player = player;

            StartCoroutine(RemoveDropAfterTime(dropItem, 40f));
            
          
        }
    }

    private IEnumerator RemoveDropAfterTime(DropItem dropItem, float delay)
    {
        yield return new WaitForSeconds(delay);
        activeDrops.Remove(dropItem);
    }

    public void OnHitByPlayer()
    {
        DropItems(useObjectPosition: true); 
        gameObject.SetActive(false);
    }
}
