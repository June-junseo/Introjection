using UnityEngine;

public class LongSwordSkill : ISkill
{
    private SkillData data;
    private PoolManager pool;
    private Transform parent;
    private Transform skillRoot;
    private Player player;
    private CharacterStats stats;

    private float cooldownTimer = 0f;

    public SkillData Data { get; set; }

    public void Init(SkillData data, PoolManager pool, Transform parent, CharacterStats stats)
    {
        this.Data = data;
        this.pool = pool;
        this.parent = parent;
        this.stats = stats;
        cooldownTimer = 0f;

        if (skillRoot == null)
        {
            GameObject rootObj = new GameObject($"{data.skillGroup}_Root");
            rootObj.transform.SetParent(parent);
            rootObj.transform.localPosition = Vector3.zero;
            skillRoot = rootObj.transform;
        }

        ClearSwords();
    }

    public void UpdateSkill()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
        }

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f)
        {
            return;
        }

        FireSwords();

        cooldownTimer = stats.GetFinalCooldown(Data.cooltime);
    }

    private void FireSwords()
    {

        ClearSwords();
        
        Vector2 forwardDir = player.GetFacingDirection();

        int count = Data.projectileCount;
        float forwardDistance = 1.5f;
        float spreadAngle = 60f;

        float playerAngle = Mathf.Atan2(forwardDir.y, forwardDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < count; i++)
        {
            GameObject swordObj = pool.Get(Data.id);

            if (swordObj == null)
            {
                continue;
            }

            float angleOffset = 0f;

            if (count > 1)
            {
                angleOffset = -spreadAngle + (2f * spreadAngle) * i / (count - 1);
            }

            Vector2 fireDir = Quaternion.Euler(0, 0, playerAngle + angleOffset) * Vector2.right;

            Vector2 spawnPos = (Vector2)player.transform.position + fireDir.normalized * forwardDistance;

            swordObj.transform.position = spawnPos;
            swordObj.transform.SetParent(skillRoot);

            LongSword sword = swordObj.GetComponent<LongSword>();
            if (sword != null)
            {
                float finalDamage = stats.GetFinalAttack() * Data.damagePercent;
                sword.Init(finalDamage, -1, fireDir, player);
            }
        }
    }


    private void ClearSwords()
    {
        for (int i = skillRoot.childCount - 1; i >= 0; i--)
        {
            pool.Release(skillRoot.GetChild(i).gameObject);
        }
    }
}
