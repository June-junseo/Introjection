using UnityEngine;

public class Monster : MonoBehaviour
{
    private NormalMonsterData normalData;
    private EliteMonsterData eliteData;
    private MonsterPool pool;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D target;
    private float hp;

    private bool isDead = false;
    private bool isKnockback = false;
    private Vector2 knockbackTarget;
    private float knockbackSpeed;
    private float knockbackTolerance = 0.05f;
    private float fadeDuration = 1f;
    private float fadeTimer;
    private float knockbackMaxTime = 0.3f;
    private float knockbackTimer;

    public bool isElite;
    public GameObject chestPrefab;
    public Player player;
    public GameObject expPrefab;
    private PoolManager poolManager;
    private Collider2D col;

    #region Init
    public void Init(NormalMonsterData data, MonsterPool pool, Rigidbody2D target, PoolManager poolManager)
    {
        normalData = data;
        eliteData = null;
        this.pool = pool;
        this.target = target;
        this.poolManager = poolManager;
        hp = normalData.baseHp;
    }

    public void Init(EliteMonsterData data, MonsterPool pool, Rigidbody2D target, PoolManager poolManager)
    {
        eliteData = data;
        normalData = null;
        this.pool = pool;
        this.target = target;
        this.poolManager = poolManager;
        hp = eliteData.baseHp;
    }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        hp = normalData != null ? normalData.baseHp : (eliteData != null ? eliteData.baseHp : 1f);
        isDead = false;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) 
        { 
            HandleFade(); return; 
        }

        if (isKnockback) 
        {
            HandleKnockback(); 
            return; 
        }

        if (target == null)
        {
            return;
        }

        Vector2 dirVec = target.position - rb.position;

        if (dirVec.sqrMagnitude > 0.01f)
        {
            rb.MovePosition(rb.position + dirVec.normalized * GetSpeed() * Time.fixedDeltaTime);
            animator?.SetBool("isWalking", true);
            spriteRenderer.flipX = dirVec.x < 0;
        }
        else
        {
            animator?.SetBool("isWalking", false);
        }
    }

    private float GetSpeed() => normalData != null ? normalData.speed : eliteData != null ? eliteData.speed : 1f;
    private float GetDamage() => normalData != null ? normalData.baseAtk : eliteData != null ? eliteData.baseAtk : 1f;

    private void HandleKnockback()
    {
        knockbackTimer -= Time.fixedDeltaTime;
        Vector2 current = rb.position;
        Vector2 next = Vector2.MoveTowards(current, knockbackTarget, knockbackSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if (Vector2.Distance(next, knockbackTarget) <= knockbackTolerance || knockbackTimer <= 0f)
        {
            EndKnockback();
        }
    }

    private void StartKnockback(Vector2 dir, float distance, float speed)
    {
        isKnockback = true;
        knockbackSpeed = speed;
        knockbackTarget = (Vector2)transform.position + dir.normalized * distance;
        knockbackTimer = knockbackMaxTime;
    }

    private void EndKnockback()
    {
        isKnockback = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void TakeDamage(float dmg, Vector2 ignoredDir, float knockbackDistance = 1f, float knockbackSpeed = 5f)
    {
        hp -= dmg;
        Vector2 knockbackDir = target != null ? ((Vector2)rb.position - (Vector2)target.position).normalized : ignoredDir;
        StartKnockback(knockbackDir, knockbackDistance, knockbackSpeed);

        if (hp <= 0)
        {
            Die(player);
        }
    }

    public void Die(Player player)
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        animator?.SetTrigger("Dead");

        if (col != null)
        {
            col.enabled = false;
        }

        if (expPrefab != null && poolManager != null)
        {
            var idComp = expPrefab.GetComponent<itemPrefabId>();
            if (idComp != null)
            {
                int expId = idComp.id;
                GameObject expObj = poolManager.Get(expId);
                if (expObj != null)
                {
                    expObj.transform.position = transform.position;
                    ExpItem expItem = expObj.GetComponent<ExpItem>();

                    if (expItem != null)
                    {
                        expItem.Init(player, poolManager);
                    }
                }
            }
        }

        isKnockback = false;
        fadeTimer = fadeDuration;

        if (isElite && chestPrefab != null)
        {
            Instantiate(chestPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void HandleFade()
    {
        fadeTimer -= Time.fixedDeltaTime;
        if (fadeTimer > 0f && spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = Mathf.Clamp01(fadeTimer / fadeDuration);
            spriteRenderer.color = c;
        }
        else
        {
            int id = normalData != null ? normalData.id : eliteData != null ? eliteData.id : 0;

            if (col != null)
            {
                col.enabled = true;
            }

            pool?.Return(id, gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            p.TakeDamage(GetDamage());
        }
    }
}