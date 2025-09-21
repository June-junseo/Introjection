using UnityEngine;

public class DaggerSkill : ISkill
{
    private PoolManager pool;
    private Transform parent;
    private Transform skillRoot;
    private CharacterStats stats;

    private float speed = 300f;

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

        ArrangeDaggers();
    }


    public void UpdateSkill()
    {
        if (skillRoot != null)
        {

            skillRoot.Rotate(Vector3.back * speed * Time.deltaTime);
        }
    }


    private void ArrangeDaggers()
    {
        if (Data.projectileCount <= 0)
        {
            return;
        }


        while (skillRoot.childCount < Data.projectileCount)
        {
            GameObject daggerObj = pool.Get(Data.id);
            if (daggerObj == null)
            {
                break;
            }

            Dagger dagger = daggerObj.GetComponent<Dagger>();

            if (dagger != null)
            {
                float finalDamage = stats.GetFinalAttack() * Data.damagePercent;
                dagger.Init(finalDamage, -1, Vector2.zero);
            }
            daggerObj.transform.SetParent(skillRoot);
        }

        while (skillRoot.childCount > Data.projectileCount)
        {
            pool.Release(skillRoot.GetChild(skillRoot.childCount - 1).gameObject);
        }

        float angleStep = 360f / Data.projectileCount;
        float radius = 2f;
        float startAngle = Data.projectileCount % 2 == 0 ? angleStep / 2f : 0f;

        for (int i = 0; i < Data.projectileCount; i++)
        {
            Transform dagger = skillRoot.GetChild(i);
            float angle = startAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            dagger.localPosition = pos;
            dagger.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
