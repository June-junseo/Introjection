using UnityEngine;

public class Dagger : MonoBehaviour
{
    private float damage;
    private int per;

    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"�浹 �߻�: {collision.name}");

        Monster monster = collision.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(damage);
            Debug.Log("���Ϳ� �浹, ������ ����");
        }
    }
}
