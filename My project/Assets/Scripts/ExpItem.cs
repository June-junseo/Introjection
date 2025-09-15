using UnityEngine;

public class ExpItem : MonoBehaviour
{
    private int expValue;
    private Player player;
    private float speed = 3f;
    private bool isCollected = false;

    public void Init(int expValue, Player player)
    {
        this.expValue = expValue;
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

        if (Vector2.Distance(transform.position, player.transform.position) < 0.1f)
        {
            Collect();
        }
    }

    private void Collect()
    {
        isCollected = true;
        //플레이어 levelUp 코드 나중에 
        Destroy(gameObject);

    }



}
