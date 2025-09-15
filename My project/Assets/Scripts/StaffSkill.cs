using UnityEngine;
using System.Linq;

public class StaffSkill : ISkill
{
    private SkillData data;
    private PoolManager pool;
    private Transform parent; // 플레이어 Transform
    private MonsterScanner monsterScanner;
    private float baseAttack = 100f;
    private float cooldownTimer = 0f;

    public void Init(SkillData data, PoolManager pool, Transform parent, MonsterScanner scanner)
    {
        this.data = data;
        this.pool = pool;
        this.parent = parent;
        this.monsterScanner = scanner;
        cooldownTimer = 0f;
    }

    public void Init(SkillData data, PoolManager pool, Transform parent)
    {
        Debug.LogWarning("StaffSkill requires MonsterScanner. Use the other Init.");
    }

    public void UpdateSkill()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        Transform[] targets = monsterScanner?.ScanMonsters();

        for (int i = 0; i < data.projectileCount; i++)
        {
            GameObject projObj = pool.Get(data.id);
            if (projObj == null) continue;

            projObj.transform.position = parent.position;

            Transform target = null;

            if (targets != null && targets.Length > 0)
                target = targets[0];

            float finalDamage = baseAttack * data.damagePercent;

            Staff staff = projObj.GetComponent<Staff>();
            if (staff != null)
            {
                if (target != null)
                {
                    staff.Init(target, finalDamage, 10f, 10f);
                }
                else
                {
                    staff.Init(parent.right, finalDamage, 10f, 10f);
                }
            }
        }

        cooldownTimer = data.cooltime;
    }
}
