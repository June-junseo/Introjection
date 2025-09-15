using UnityEngine;

public class Dagger : MonoBehaviour
{
    private float damage;
    private int per;
    private Vector2 moveDir;

    public void Init(float damage, int per, Vector2 dir)
    {
        this.damage = damage;
        this.per = per;
        this.moveDir = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Monster monster = collision.GetComponent<Monster>();
        if (monster != null)
        {
            Vector2 knockbackDir = moveDir;
            float knockbackDistance = 1f;
            float knockbackSpeed = 4f;

            monster.TakeDamage(damage, knockbackDir, knockbackDistance, knockbackSpeed);
        }
    }
}
