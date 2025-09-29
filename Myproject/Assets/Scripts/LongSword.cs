using UnityEngine;

public class LongSword : MonoBehaviour
{
    private float damage;
    private int per;
    private Player player;

    [SerializeField] private ParticleSystem attackEffect;
    private Collider2D swordCollider;

    private void Awake()
    {
        swordCollider = GetComponent<Collider2D>();

        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    public void Init(float damage, int per, Vector2 dir, Player player)
    {
        this.damage = damage;
        this.per = per;
        this.player = player;

        gameObject.SetActive(true);

        if (attackEffect != null)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            attackEffect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            attackEffect.Play();
        }

        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }

        Invoke(nameof(DeactivateSword), 0.2f);
    }

    private void Update()
    {
        if (attackEffect != null && swordCollider != null)
        {
            swordCollider.transform.position = attackEffect.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BossMonster boss = collision.GetComponent<BossMonster>();
        if (boss != null)
        {
            boss.TakeDamage(damage, Vector2.zero, 0f, 0f);
        }
        Monster monster = collision.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(damage, Vector2.zero, 0f, 0f);
        }

        BreakableObject breakable = collision.GetComponent<BreakableObject>();

        if (breakable != null)
        {
            breakable.SetPlayer(player);
            breakable.OnHitByPlayer();
        }
    }

    private void DeactivateSword()
    {
        if (attackEffect != null)
        {
            attackEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }

        gameObject.SetActive(false);
    }
}
