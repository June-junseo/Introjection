using System.Linq;
using UnityEngine;

public class MonsterScanner : MonoBehaviour
{
    public float scanDistance = 10f;
    public LayerMask monsterLayer;
    public int maxScanCount = 4;

    public Transform[] ScanMonsters()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, scanDistance, monsterLayer);

        if (hits.Length == 0)
        {
            return null;
        }

       var ordered = hits.OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).Take(maxScanCount);

        return ordered.Select(x => x.transform).ToArray();
    }

}
