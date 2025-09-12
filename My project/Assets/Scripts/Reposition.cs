using UnityEngine;

public class Reposition : MonoBehaviour
{
    public Player player;

    private Collider2D coll;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.GetComponent<Player>();
        }
    }


    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!collision.CompareTag("Area"))
    //    {
    //        return;
    //    }

    //    Vector3 playerPos = player.transform.position;
    //    Vector3 myPos = transform.position;

    //    float diffX = Mathf.Abs(playerPos.x - myPos.x);
    //    float diffY = Mathf.Abs(playerPos.y - myPos.y);

    //    Vector3 playerDir = player.vec;

    //    float dirX = playerDir.x > 0 ? 1 : -1;
    //    float dirY = playerDir.y > 0 ? 1 : -1;

    //    switch (transform.tag)
    //    {
    //        case "Ground":

    //            if (diffX > diffY)
    //            {
    //                transform.Translate(Vector3.right * dirX * 80);
    //            }
    //            else if (diffX < diffY)
    //            {
    //                transform.Translate(Vector3.up * dirY * 80);
    //            }
    //            //else
    //            //{
    //            //    transform.Translate(Vector3.right * dirX * 80);
    //            //    transform.Translate(Vector3.up * dirY * 80);
    //            //}
    //            break;

    //        case "Monster":
    //            if (coll.enabled)
    //            {
    //                transform.Translate(playerDir * 30 +
    //                    new Vector3(Random.Range(-3f, 3f),
    //                                Random.Range(-3f, 3f), 0f));
    //            }
    //            break;
    //    }
    //}
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = player.transform.position;
        Vector3 myPos = transform.position;

        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = player.vec;
        if (playerDir == Vector3.zero) return; // 플레이어가 멈춰있을 때 이동 방지

        float dirX = playerDir.x > 0 ? 1 : (playerDir.x < 0 ? -1 : 0);
        float dirY = playerDir.y > 0 ? 1 : (playerDir.y < 0 ? -1 : 0);

        float tileSize = 40f; // 실제 타일 크기로 변경 필요

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * tileSize);
                else if (diffX < diffY)
                    transform.Translate(Vector3.up * dirY * tileSize);
                else
                    transform.Translate(new Vector3(dirX, dirY, 0) * tileSize); // 대각선 이동 처리
                break;

            case "Monster":
                if (coll.enabled)
                {
                    transform.Translate(playerDir.normalized * 30 +
                        new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}
