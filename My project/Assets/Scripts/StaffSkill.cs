using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.GlobalIllumination;


//public class Skill : MonoBehaviour
//{
//    private SkillData data;
//    private PoolManager pool;
//    private Transform parent;
//    private MonsterScanner monsterScanner;
//    private float cooldownTimer;

//    public void Init(SkillData data, PoolManager pool, Transform parent, MonsterScanner scanner)
//    {
//        this.data = data;
//        this.pool = pool;
//        this.parent = parent;
//        this.monsterScanner = scanner;
//        cooldownTimer = 0f;
//    }

//    public virtual void UpdateSkill()
//    {
//        cooldownTimer -= Time.deltaTime;
//    }
//}


public class StaffSkill : ISkill
{
    private SkillData data;
    private PoolManager pool;
    private Transform parent; 
    private MonsterScanner monsterScanner;
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
        if(player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f)
        {
            return;
        }

        for (int i = 0; i < data.projectileCount; i++)
        {
            GameObject projObj = pool.Get(data.id);
            if (projObj == null)
            {
                continue;
            }

            projObj.transform.position = parent.position;

            float finalDamage = baseAttack * data.damagePercent;

            Staff staff = projObj.GetComponent<Staff>();
          
              if (staff != null)
              {
                Vector3 dir = player.vec;

                if (dir == Vector3.zero)
                {
                    dir = Vector3.right;
                }

                dir = dir.normalized;

                float angleOffset = (i - data.projectileCount / 2f) * 10f;
                dir = Quaternion.Euler(0, 0, angleOffset) * dir;

                staff.Init(dir, finalDamage, 10f, 10f);
            }
        }
        cooldownTimer = data.cooltime;
    }

}
