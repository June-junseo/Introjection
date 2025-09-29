using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public BossMonsterData data;
    private Rigidbody2D rb;
    private SPUM_Prefabs spum;
    private Transform spumRoot;
    private Player player;
    private StaffSkill staffSkill;
    private bool isDead = false;
    private float hp;

    private bool isKnockback = false;
    private Vector2 knockbackTarget;
    private float knockbackSpeed;
    private float knockbackTolerance = 0.05f;
    private float knockbackMaxTime = 0.3f;
    private float knockbackTimer;
    private Rigidbody2D target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spum = GetComponentInChildren<SPUM_Prefabs>();
        spumRoot = spum.transform;
        player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
    }

    public void Init(BossMonsterData bossData, Player playerTarget, PoolManager poolManager)
    {
        data = bossData;
        player = playerTarget;

        hp = data.baseHp;

        spum.PopulateAnimationLists();
        spum.OverrideControllerInit();
        spum.PlayAnimation(PlayerState.IDLE, 0);

        staffSkill = new StaffSkill();
        staffSkill.Init(data.skillData, poolManager, transform, player.GetComponent<CharacterStats>());
    }


    private void Update()
    {
        if (isDead)
        {
            return;
        }

        Vector2 dir = (player.transform.position - transform.position).normalized;
        spumRoot.localScale = dir.x > 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);

        spum.PlayAnimation(PlayerState.MOVE, 0);
        staffSkill?.UpdateSkill();
    }

    private void FixedUpdate()
    {
        if (isDead || player == null)
        {
            return;
        }

        if (isKnockback)
        {
            HandleKnockback();
            return; 
        }

        Vector2 dir = (player.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * data.speed * Time.fixedDeltaTime);
    }

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
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;

        spum.PlayAnimation(PlayerState.DEATH, 0);
        Destroy(gameObject, 3f);
    }
}
