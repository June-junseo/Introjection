using Kinnly;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Reposition : MonoBehaviour
{
    public Player player;

    private Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerPos = player.transform.position;
        Vector3 myPos = transform.position;

        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = player.vec;

        float dirX = playerDir.x > 0 ? 1 : -1;
        float dirY = playerDir.y > 0 ? 1 : -1;

        switch(transform.tag)
        {
            case "Ground":
                if(diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 60);
                }
                else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 60);
                }
                else
                {
                    transform.Translate(Vector3.right * dirX * 60);
                    transform.Translate(Vector3.up * dirY * 60);
                }
                break;
            case "Monster":
                if(coll.enabled)
                {
                    transform.Translate(playerDir * 30 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}
