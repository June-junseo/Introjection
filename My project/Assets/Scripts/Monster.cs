using UnityEngine;

public class Monster : MonoBehaviour
{
    private MonsterData data;
    private MonsterPool pool;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D target; 
    private float hp;

    private bool isKnockback = false;
    private Vector2 knockbackTarget;
    private float knockbackSpeed = 5f;
    private float knockbackTolerance = 0.05f;

    private bool isDead = false;
    private float fadeDuration = 1.0f;   
    private float fadeTimer = 0f;


    public void Init(MonsterData data, MonsterPool pool, Rigidbody2D target)
    {
        this.data = data;
        this.pool = pool;
        this.target = target;
        hp = data.maxHp;
    }

    public void Init(Player player)
    {
        GetComponent<Reposition>().player = player;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (data != null)
        {
            hp = data.maxHp;
        }

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
            HandleFade();
            return;
        }
        if (isKnockback)
        {
            Vector2 current = rb != null ? rb.position : (Vector2)transform.position;
            Vector2 next = Vector2.MoveTowards(current, knockbackTarget, knockbackSpeed * Time.fixedDeltaTime);

            if (rb != null)
                rb.MovePosition(next);
            else
                transform.position = next;


            if (Vector2.Distance(next, knockbackTarget) <= knockbackTolerance)
            {
                EndKnockback();
            }

            return; 
        }


        if (target == null) return;

        Vector2 dirVec = target.position - rb.position;
        if (dirVec.sqrMagnitude > 0.01f)
        {
            Vector2 nextVec = dirVec.normalized * data.speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + nextVec);

            animator?.SetBool("isWalking", true);

            if (dirVec.x != 0)
                spriteRenderer.flipX = dirVec.x < 0;
        }
        else
        {
            animator?.SetBool("isWalking", false);
        }
    }

    private void HandleFade()
    {
        if (fadeTimer > 0f)
        {
            fadeTimer -= Time.fixedDeltaTime;

            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = Mathf.Clamp01(fadeTimer / fadeDuration);
                spriteRenderer.color = c;
            }
        }
        else
        {
            pool.Return(data.type, gameObject);
        }
    }


    private void StartKnockback(Vector2 dir, float distance, float speed, float duration = 0f)
    {

        isKnockback = true;
        knockbackSpeed = speed;
        knockbackTarget = (Vector2)transform.position + dir.normalized * distance;

   
    }

    private void EndKnockback()
    {
        isKnockback = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void TakeDamage(float dmg, Vector2 ignoredDir, float knockbackDistance = 1f, float knockbackSpeed = 5f, float knockbackDuration = 0f)
    {
        hp -= dmg;
        Debug.Log($"(µ¥¹ÌÁö {dmg})");

        if (target != null)
        {
            Vector2 knockbackDir = ((Vector2)rb.position - (Vector2)target.position).normalized;
            StartKnockback(knockbackDir, knockbackDistance, knockbackSpeed, knockbackDuration);
        }
        else
        {
            StartKnockback(ignoredDir, knockbackDistance, knockbackSpeed, knockbackDuration);
        }

        if (hp <= 0)
        {
            Die();
        }
    } 

    public void Die()
    {
        if (isDead)
        {
            return;
        }

            isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        isKnockback = false;
        fadeTimer = fadeDuration;
    }
}
