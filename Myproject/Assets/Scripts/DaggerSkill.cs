using UnityEngine;

public class DaggerSkill : ISkill
{
    public SkillData Data { get; set; }

    private PoolManager pool;
    private Transform parent;
    private Transform skillRoot;
    private CharacterStats stats;

    private float speed = 250f;
    private float cooldownTimer = 0f;
    private float durationTimer = 0f;
    private bool isActive = false;

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

        ResetSkill(); // 초기화
    }

    public void UpdateSkill()
    {
        if (skillRoot != null)
            skillRoot.Rotate(Vector3.back * speed * Time.deltaTime);

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
        isActive = true;
        Debug.Log($"[DaggerSkill] 발동! duration: {Data.duration}s, 다음 쿨타임: {Data.cooltime}s");
    }

    private void ArrangeDaggers()
    {
        ClearDaggers();

        if (skillRoot == null || Data.projectileCount <= 0)
        {
            return;
        }

        int totalCount = stats.GetFinalProjectileCount(Data.projectileCount);
        float angleStep = 360f / totalCount;
        float radius = 3.5f;
        float startAngle = totalCount % 2 == 0 ? angleStep / 2f : 0f;

        for (int i = 0; i < totalCount; i++)
        {
            GameObject daggerObj = pool.Get(Data.id);
            if (daggerObj == null)
            {
                continue;
            }

            Dagger dagger = daggerObj.GetComponent<Dagger>();
            if (dagger != null)
            {
                float finalDamage = stats.CalculateDamage(Data.damagePercent);
                dagger.Init(finalDamage, -1, Vector2.zero, null);
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

    private void ClearDaggers()
    {
        if (skillRoot == null)
        {
            return;
        }

        for (int i = skillRoot.childCount - 1; i >= 0; i--)
        {
            Transform child = skillRoot.GetChild(i);
            child.SetParent(null);
            pool.Release(child.gameObject);
        }
    }

    public void ResetSkill()
    {
        isActive = false;
        cooldownTimer = 0f;
        durationTimer = 0f;

        if (skillRoot != null)
        {
            for (int i = skillRoot.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(skillRoot.GetChild(i).gameObject);
            }

            skillRoot.localPosition = Vector3.zero;
            skillRoot.localRotation = Quaternion.identity;
        }
    }
}
