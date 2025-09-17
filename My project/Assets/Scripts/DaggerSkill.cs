using UnityEngine;

public class DaggerSkill : ISkill
{
    private SkillData data;
    private PoolManager pool;
    private Transform parent;
    private float speed = 300f;

    private float baseAttack = 30f; 

    public void Init(SkillData data, PoolManager pool, Transform parent)
    {
        this.data = data;
        this.pool = pool;
        this.parent = parent;
        ArrangeDaggers();
    }
    public void UpdateSkill()
    {
        parent.Rotate(Vector3.back * speed * Time.deltaTime);
    }

    private void ArrangeDaggers()
    {
        if (data.projectileCount <= 0)
        {
            return;
        }

        float angleStep = 360f / data.projectileCount;
        float radius = 1.5f;

        for (int i = 0; i < data.projectileCount; i++)
        {
            float angle = i * angleStep;
            GameObject daggerObj = pool.Get(data.id);

            if (!daggerObj)
            {
                continue;
            }

            Transform dagger = daggerObj.transform;
            dagger.SetParent(parent);

            dagger.localRotation = Quaternion.Euler(0, 0, angle);
            dagger.localPosition = dagger.up * radius;

            float finalDamage = baseAttack * data.damagePercent;

            Vector2 moveDir = dagger.up;

            Dagger daggerComponent = dagger.GetComponent<Dagger>();
            if (daggerComponent != null)
            {
                daggerComponent.Init(finalDamage, -1, moveDir);
            }
        }
    }
}
