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

        for (int i = skillRoot.childCount - 1; i >= 0; i--)
        {
            pool.Release(skillRoot.GetChild(i).gameObject);
        }
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

        Vector2 direction = player.vec;
        if (direction == Vector2.zero)
        {
            direction = Vector2.right;
        }

        GameObject swordObj = pool.Get(Data.id);

        if (swordObj != null)
        {
            swordObj.transform.position = player.transform.position;
            swordObj.transform.SetParent(skillRoot);

            LongSword sword = swordObj.GetComponent<LongSword>();
            if (sword != null)
            {

                float finalDamage = stats.GetFinalAttack() * Data.damagePercent;
                sword.Init(finalDamage, -1, direction);
            }
        }

        cooldownTimer = stats.GetFinalCooldown(Data.cooltime);
    }
}
