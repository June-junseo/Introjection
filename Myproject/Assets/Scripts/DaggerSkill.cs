using UnityEngine;

public class DaggerSkill : ISkill
{
    private PoolManager pool;
    private Transform parent;
    private Transform skillRoot;
    private CharacterStats stats;
    private Player player;

    private float speed = 250f;

    private float cooldownTimer = 0f;
    private float durationTimer = 0f; 

    public SkillData Data { get; set; }

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

        cooldownTimer = 0f;
        durationTimer = 0f;
    }

    public void UpdateSkill()
    {
        if (skillRoot != null)
        {
            skillRoot.Rotate(Vector3.back * speed * Time.deltaTime);
        }

        if (durationTimer > 0f)
        {
            durationTimer -= Time.deltaTime;

            if (durationTimer <= 0f)
            {
                ClearDaggers();
                cooldownTimer = Data.cooltime;
            }

            return; 
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        TriggerSkill();
    }

    private void TriggerSkill()
    {
        ArrangeDaggers();

        durationTimer = Data.duration;
        Debug.Log($"[DaggerSkill] 발동! duration: {Data.duration}s, 다음 쿨타임: {Data.cooltime}s");
    }

    private void ClearDaggers()
    {
        if (skillRoot == null)
        {
            return;
        }

        int count = skillRoot.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            Transform child = skillRoot.GetChild(i);
            child.SetParent(null);
            pool.Release(child.gameObject);
        }
    }

    private void ArrangeDaggers()
    {
        if (skillRoot == null || Data.projectileCount <= 0)
        {
            return;
        }

        ClearDaggers();

        float angleStep = 360f / Data.projectileCount;
        float radius = 3.5f;
        float startAngle = Data.projectileCount % 2 == 0 ? angleStep / 2f : 0f;

        for (int i = 0; i < Data.projectileCount; i++)
        {
            GameObject daggerObj = pool.Get(Data.id);

            if (daggerObj == null)
            {
                continue;
            }

            Dagger dagger = daggerObj.GetComponent<Dagger>();

            if (dagger != null)
            {
                float finalDamage = stats.GetFinalAttack() * Data.damagePercent;
                dagger.Init(finalDamage, -1, Vector2.zero, player);
            }

            daggerObj.transform.SetParent(skillRoot, false);

            float angle = startAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            daggerObj.transform.localPosition = pos;
            daggerObj.transform.localRotation = Quaternion.Euler(0, 0, angle);

            daggerObj.SetActive(true);
        }
    }
}
