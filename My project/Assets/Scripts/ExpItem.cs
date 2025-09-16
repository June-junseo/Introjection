using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public int expValue =1;
    private float speed = 1f;
    private Player player;
    private bool isCollected = false;

    public void Init(Player player)
    {
        this.player = player;
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

        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            Collect(p);
        }

    }

    private void Collect(Player player)
    {
        isCollected = true;
        player.AddExp(expValue);
        Destroy(gameObject);
    }



}
