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

    public void SetPlayer(Player p) => player = p;

    private void Awake()
    {
        if (csvFile == null)
        {
            Debug.LogWarning("CSV 파일이 없습니다.");
            return;
        }

        var drops = CSVLoader.LoadCSV<DropData>(csvFile);
        dropInfo = drops.Find(d => d.ID == ID && d.STAGE == stage);

        if (dropInfo == null)
        {
            Debug.LogWarning($"DropData not found for ID {ID}, STAGE {stage}");
        }
    }

    public void OnHitByPlayer()
    {
        DropItems();
        gameObject.SetActive(false);
    }

    private void DropItems()
    {
        if (dropInfo == null || player == null)
        {
            return;
        }

        if (Random.value <= dropInfo.DROP1_RATE && hpItemPrefab != null)
        {
            int hpAmount = Random.Range(1, dropInfo.DROP1_MaxCount + 1);
            SpawnDropItem(DropItem.ItemType.HP, hpAmount);
        }

        if (Random.value <= dropInfo.DROP2_RATE && goldItemPrefab != null)
        {
            int goldAmount = dropInfo.DROP2_COUNT;
            SpawnDropItem(DropItem.ItemType.Gold, goldAmount);
        }
    }

    private void SpawnDropItem(DropItem.ItemType type, int amount)
    {
        GameObject prefab = type == DropItem.ItemType.HP ? hpItemPrefab : goldItemPrefab;
        if (prefab == null)
        {
            return;
        }

        GameObject dropObj = Instantiate(prefab, transform.position, Quaternion.identity);
        DropItem dropItem = dropObj.GetComponent<DropItem>();
        if (dropItem != null)
        {
            dropItem.type = type;
            dropItem.amount = amount;
            dropItem.player = player; 
        }
    }

}
