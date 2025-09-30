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
    private PoolManager poolManager;

    public int damagePopupId = 8002;

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
        this.poolManager = poolManager;

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

        Vector2 dir = (player.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * data.speed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Die();
        }
        ShowDamagePopup(dmg);
    }

    private void ShowDamagePopup(float dmg)
    {
        if (poolManager == null)
        {
            Debug.LogWarning("PoolManager X");
            return;
        }

        GameObject popupObj = poolManager.Get(damagePopupId);
        if (popupObj == null)
        {
            return;
        }

        GameObject canvasObj = GameObject.FindGameObjectWithTag("Canvas");
        if (canvasObj == null)
        {
            Debug.LogWarning("Canvas X");
            return;
        }
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        popupObj.transform.SetParent(canvas.transform, false);

        Vector3 worldPos = transform.position + Vector3.up * 0.5f;
        DamagePopup popup = popupObj.GetComponent<DamagePopup>();
        popup.Play(dmg.ToString(), Color.red, worldPos, poolManager, damagePopupId);
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
