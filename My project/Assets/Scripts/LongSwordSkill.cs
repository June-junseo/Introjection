using UnityEngine;

public class LongSwordSkill : ISkill
{
    private SkillData data;
    private PoolManager pool;
    private Transform parent;
    private Player player;
    private float baseAttack = 100f;
    private float cooldownTimer = 0f;

    public void Init(SkillData data, PoolManager pool, Transform parent)
    {
        this.data = data;
        this.pool = pool;
        this.parent = parent;
        cooldownTimer = 0f;
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

        GameObject swordObj = pool.Get(data.id);
        if (swordObj != null)
        {
            swordObj.transform.position = player.transform.position;
            swordObj.transform.parent = parent;

            LongSword sword = swordObj.GetComponent<LongSword>();
            if (sword != null)
            {
                sword.Init(baseAttack * data.damagePercent, -1, direction);
            }
        }

        cooldownTimer = data.cooltime;
    }
}
