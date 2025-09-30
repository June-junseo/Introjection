using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum ItemType { HP, Gold}
    public ItemType type;
    public int hpAmount = 10;
    public int goldAmount = 1;
    [HideInInspector] public Player player;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            if (type == ItemType.HP)
            {
                int healAmount = 10;
                player.currentHp += healAmount;
                player.currentHp = Mathf.Clamp(player.currentHp, 0, player.maxHp);
                player.Heal(healAmount);
            }
            else if (type == ItemType.Gold)
            {
                player.AddGold(1);
            }

            Destroy(gameObject);
        }
    }
}
