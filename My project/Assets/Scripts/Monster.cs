using UnityEngine;

public class Monster : MonoBehaviour
{
    private MonsterData data;
    private MonsterPool pool;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D target; // Player
    private float hp;

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
        spriteRenderer = GetComponent<SpriteRenderer>(); // flipX를 위해 필요
    }

    private void OnEnable()
    {
        if (data != null)
            hp = data.maxHp;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 dirVec = target.position - rb.position;

        if (dirVec.sqrMagnitude > 0.01f)
        {
            Vector2 nextVec = dirVec.normalized * data.speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + nextVec);

            animator.SetBool("isWalking", true);

            if (dirVec.x != 0)
                spriteRenderer.flipX = dirVec.x < 0;
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        Debug.Log($"몬스터 체력: {hp}");

        if (hp <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        pool.Return(data.type, gameObject);
    }
}
