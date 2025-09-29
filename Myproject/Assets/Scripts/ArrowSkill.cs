using UnityEngine;

public class ArrowSkill : ISkill
{
    public SkillData Data { get; set; }

    private PoolManager pool;
    private Transform parent;
    private Transform skillRoot;
    private Player player;
    private CharacterStats stats;

    private float cooldownTimer = 0f;

    public void Init(SkillData data, PoolManager pool, Transform parent, CharacterStats stats)
    {
        this.Data = data;
        this.pool = pool;
        this.parent = parent;
        this.stats = stats;

        if (skillRoot == null)
        {
            GameObject rootObj = new GameObject($"{data.skillGroup}_Root");
            rootObj.transform.SetParent(parent);
            rootObj.transform.localPosition = Vector3.zero;
            skillRoot = rootObj.transform;
        }

        player ??= GameObject.FindWithTag("Player")?.GetComponent<Player>();
        ResetSkill();
    }

    public void UpdateSkill()
    {
        if (player == null)
        {
            return;
        }

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f)
        {
            return;
        }

        int totalCount = stats.GetFinalProjectileCount(Data.projectileCount);

        for (int i = 0; i < totalCount; i++)
        {
            GameObject projObj = pool.Get(Data.id);
            if (projObj == null)
            {
                continue;
            }

            projObj.transform.SetParent(skillRoot);
            projObj.transform.position = parent.position;

            Arrow arrow = projObj.GetComponent<Arrow>();
            if (arrow != null)
            {
                Vector3 dir = player.GetFacingDirection();
                float angleOffset = (i - (Data.projectileCount - 1) / 2f) * 10f;
                dir = Quaternion.Euler(0, 0, angleOffset) * dir;

                arrow.Init(dir.normalized, stats.CalculateDamage(Data.damagePercent), 10f, 10f, player);
            }
        }

        cooldownTimer = stats.GetFinalCooldown(Data.cooltime);
    }

    public void ResetSkill()
    {
        cooldownTimer = 0f;

        if (skillRoot != null)
        {
            for (int i = skillRoot.childCount - 1; i >= 0; i--)
            {
                pool.Release(skillRoot.GetChild(i).gameObject);
            }
            skillRoot.localPosition = Vector3.zero;
            skillRoot.localRotation = Quaternion.identity;
        }
    }
}
