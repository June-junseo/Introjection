using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum ItemType { HP, Gold }
    public ItemType type;
    public int amount = 10;
    [HideInInspector] public Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            if (type == ItemType.HP)
            {
                player.currentHp += amount;
                player.currentHp = Mathf.Clamp(player.currentHp, 0, player.maxHp);

                player.Heal(amount);
            }
            else if (type == ItemType.Gold)
            {
                player.gold += amount;
            }

            Destroy(gameObject);
        }
    }

}
