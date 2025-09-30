using UnityEngine;

public class Rosario : MonoBehaviour
{
    private float damage;
    private Player player;

    public void Init(float damage, Vector2 dir, Player player)
    {
        this.damage = damage;
        this.player = player;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        BossMonster boss = collision.GetComponent<BossMonster>();
        if (boss != null)
        {
            boss.TakeDamage(damage, Vector2.zero, 0f, 0f);
        }
        Debug.Log($"Rosario Trigger: {collision.name}");

        Monster monster = collision.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("∏ÛΩ∫≈Õ √Êµπµ , µ•πÃ¡ˆ ¡‹");
            monster.TakeDamage(damage, Vector2.zero, 0f, 0f);
        }

        BreakableObject breakable = collision.GetComponent<BreakableObject>();
        if (breakable != null)
        {
            breakable.SetPlayer(player);
            breakable.OnHitByPlayer();
        }
    }
}
