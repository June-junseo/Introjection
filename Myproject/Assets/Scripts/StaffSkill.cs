using UnityEngine;

public class StaffSkill : ISkill
{
    private PoolManager pool;
    private Transform parent;
    private Transform skillRoot; // Àü¿ë root
    private Player player;
    private float baseAttack = 100f;
    private float cooldownTimer = 0f;

    public SkillData Data { get; set; }

    public void Init(SkillData data, PoolManager pool, Transform parent)
    {
        this.Data = data;
        this.pool = pool;
        this.parent = parent;

        if (skillRoot == null)
        {
            GameObject rootObj = new GameObject($"{data.skillGroup}_Root");
            rootObj.transform.SetParent(parent);
            rootObj.transform.localPosition = Vector3.zero;
            skillRoot = rootObj.transform;
        }

        cooldownTimer = 0f;
    }

    public void UpdateSkill()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").GetComponent<Player>();

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        for (int i = 0; i < Data.projectileCount; i++)
        {
            GameObject projObj = pool.Get(Data.id);
            if (projObj == null) continue;

            projObj.transform.SetParent(skillRoot);
            projObj.transform.position = parent.position;

            float finalDamage = baseAttack * Data.damagePercent;
            Staff staff = projObj.GetComponent<Staff>();

            if (staff != null)
            {
                Vector3 dir = player.vec;
                if (dir == Vector3.zero) dir = Vector3.right;
                dir = dir.normalized;

                float angleOffset = (i - Data.projectileCount / 2f) * 10f;
                dir = Quaternion.Euler(0, 0, angleOffset) * dir;

                staff.Init(dir, finalDamage, 10f, 10f);
            }
        }

        cooldownTimer = Data.cooltime;
    }
}
