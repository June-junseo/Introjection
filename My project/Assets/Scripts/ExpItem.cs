using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public int expValue =1;
    private float speed = 1f;
    private Player player;
    private bool isCollected = false;
    private PoolManager pool;

    public void Init(Player player, PoolManager pool)
    {
        this.player = player;
        this.pool = pool;
        isCollected = false;
        gameObject.SetActive(true);
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }

    private void Update()
    {
        if(player == null || isCollected)
        {
            return;
        }

        Vector2 dir = (player.transform.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isCollected)
        {
            return;
        }

        Debug.Log("ExpItem Trigger: " + collision.name);

        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            Collect(p);
        }
    }

    private void Collect(Player player)
    {
        isCollected = true;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        player.AddExp(expValue);

        pool.Release(gameObject);
    }
}
