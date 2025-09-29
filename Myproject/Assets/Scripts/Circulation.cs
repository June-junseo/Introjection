using UnityEngine;

public class Circulation : MonoBehaviour
{
    private float damage;
    private int per;
    private Vector2 moveDir;
    private Player player;

    public void Init(float damage, int per, Vector2 dir, Player player)
    {
        this.damage = damage;
        this.per = per;
        this.moveDir = dir.normalized;
        this.player = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BossMonster boss = collision.GetComponent<BossMonster>();
        if (boss != null)
        {

            Vector2 knockbackDir = transform.right;
            float knockbackDistance = 1.2f;
            float knockbackSpeed = 6f;
            boss.TakeDamage(damage, knockbackDir, knockbackDistance, knockbackSpeed);
        }
        Monster monster = collision.GetComponent<Monster>();
        if (monster != null)
        {
            Vector2 knockbackDir = moveDir;
            float knockbackDistance = 1f;
            float knockbackSpeed = 4f;

            monster.TakeDamage(damage, knockbackDir, knockbackDistance, knockbackSpeed);
        }

        BreakableObject breakable = collision.GetComponent<BreakableObject>();

        if (breakable != null)
        {
            breakable.SetPlayer(player);
            breakable.OnHitByPlayer();
        }
    }
}
