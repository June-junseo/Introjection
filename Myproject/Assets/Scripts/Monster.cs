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
    private float spawnTime;

    public bool isElite;
    public GameObject chestPrefab;
    public Player player;
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

        spawnTime = Time.time;
        hp = GetScaledHp();

    }

    public void Init(EliteMonsterData data, MonsterPool pool, Rigidbody2D target, PoolManager poolManager)
    {
        eliteData = data;
        normalData = null;
        this.pool = pool;
        this.target = target;
        this.poolManager = poolManager;
        hp = eliteData.baseHp;

        spawnTime = Time.time;
        hp = GetScaledHp();
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

    private float GetScaledHp()
    {
        float baseHp = normalData != null ? normalData.baseHp : eliteData != null ? eliteData.baseHp : 1f;
        float elapsedMinutes = (Time.time - spawnTime) / 60f;
        float growthRate = 0.2f; 
        return baseHp * (1f + growthRate * elapsedMinutes);
    }

    private float GetScaledDamage()
    {
        float baseAtk = normalData != null ? normalData.baseAtk : eliteData != null ? eliteData.baseAtk : 1f;
        float elapsedMinutes = (Time.time - spawnTime) / 60f;
        float growthRate = 0.15f; 
        return baseAtk * (1f + growthRate * elapsedMinutes);
    }

    private void FixedUpdate()
    {
        if (isDead) 
        { 
            HandleFade();
            return; 
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
        Vector2 next = Vector2.MoveTowards(rb.position, knockbackTarget, knockbackSpeed * Time.fixedDeltaTime);
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

    private void Die(Player player)
    {
        if (isDead)
        {
            return;
        }
        isDead = true;

        animator?.SetTrigger("Dead");
        if (col != null) col.enabled = false;

        if (normalData != null)
        {
            TryDrop_Normal_Exclusive(normalData.drop1, normalData.drop1Rate, normalData.drop2, normalData.drop2Rate, player);
        }
        else if (eliteData != null)
        {
            TryDrop_Elite_Exclusive(eliteData.drop1, eliteData.drop1MinCount, eliteData.drop1MaxCount, eliteData.drop2MinCount,
                                    eliteData.drop2, eliteData.drop2MinCount, eliteData.drop2MaxCount, eliteData.drop2MinCount, player);
        }

        isKnockback = false;
        fadeTimer = fadeDuration;

        if (isElite && chestPrefab != null)
        {
            Instantiate(chestPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void TryDrop_Normal_Exclusive(string drop1, float drop1Rate, string drop2, float drop2Rate, Player player)
    {
        float r = Random.value; 
        if (r < drop1Rate)
        {
            SpawnDropFromPool(drop1, player);
        }
        else if (r < drop1Rate + drop2Rate)
        {
            SpawnDropFromPool(drop2, player);
        }

    }

    private void TryDrop_Elite_Exclusive(string drop1, int min1, int max1, float drop1Rate,
                                         string drop2, int min2, int max2, float drop2Rate, Player player)
    {
        float r = Random.value;
        float totalRate = Mathf.Clamp01(drop1Rate + drop2Rate);

        if (r < drop1Rate)
        {
            int count = Random.Range(min1, max1 + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnDropFromPool(drop1, player);
            }
        }
        else if (r < totalRate)
        {
            int count = Random.Range(min2, max2 + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnDropFromPool(drop2, player);
            }
        }
    }


    private void SpawnDropFromPool(string itemName, Player player)
    {
        GameObject prefab = Resources.Load<GameObject>(itemName);
        if (prefab == null)
        {
            Debug.LogWarning($"Drop prefab {itemName} not found in Resources!");
            return;
        }

        var idComp = prefab.GetComponent<itemPrefabId>();
        if (idComp != null && poolManager != null)
        {
            GameObject dropObj = poolManager.Get(idComp.id);
            if (dropObj != null)
            {
                dropObj.transform.position = transform.position + (Vector3)(Random.insideUnitCircle * 0.3f);
                ExpItem expItem = dropObj.GetComponent<ExpItem>();
                if (expItem != null)
                {
                    expItem.Init(player, poolManager);
                }
            }
        }
        else
        {
            Instantiate(prefab, transform.position + (Vector3)(Random.insideUnitCircle * 0.3f), Quaternion.identity);
        }
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
