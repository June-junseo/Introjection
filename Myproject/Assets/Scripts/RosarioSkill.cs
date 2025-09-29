using UnityEngine;

public class RosarioSkill : ISkill
{
    public SkillData Data { get; set; }

    private PoolManager pool;
    private Transform parent;
    private Player player;
    private CharacterStats stats;

    private GameObject rosarioObj;
    private Rosario rosario;
    private CircleCollider2D rosarioCollider;

    public void Init(SkillData data, PoolManager pool, Transform parent, CharacterStats stats)
    {
        this.Data = data;
        this.pool = pool;
        this.parent = parent;
        this.stats = stats;

        player ??= GameObject.FindWithTag("Player")?.GetComponent<Player>();

        if (rosarioObj == null)
        {
            rosarioObj = pool.Get(data.id);
            rosarioObj.transform.SetParent(player.transform);
            rosarioObj.transform.localPosition = Vector3.zero;

            Rigidbody2D rb = rosarioObj.GetComponent<Rigidbody2D>();
            if (rb == null) rb = rosarioObj.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;

            rosarioCollider = rosarioObj.GetComponent<CircleCollider2D>();
            rosario = rosarioObj.GetComponent<Rosario>();
        }

        ResetSkill();
        UpdateRosario();
    }

    public void UpdateSkill()
    {
        UpdateRosario();
    }

    private void UpdateRosario()
    {
        if (rosario == null)
        {
            return;
        }

        float finalDamage = stats.CalculateDamage(Data.damagePercent);
        rosario.Init(finalDamage, Vector2.zero, player);

        float range = Data.range;
        rosarioObj.transform.localScale = new Vector3(range, range, 1f);

        if (rosarioCollider != null)
        {
            rosarioCollider.radius = range * 0.5f;
        }


        if (!rosarioObj.activeSelf)
        {
            rosarioObj.SetActive(true);
        }
    }


    public void ResetSkill()
    {
        if (rosarioObj != null)
        {
            rosarioObj.SetActive(false);
            rosarioObj.transform.localPosition = Vector3.zero;
            rosarioObj.transform.localRotation = Quaternion.identity;
        }
    }
}
