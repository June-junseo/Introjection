using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public SkillData skillData; 
    public PoolManager pool;

    private ISkill currentSkill;

    private void Start()
    {
        if (skillData == null || pool == null)
        {
            return;
        }

        currentSkill = SkillFactory.CreateSkill(skillData);
        currentSkill?.Init(skillData, pool, transform);
    }

    private void Update()
    {
        currentSkill?.UpdateSkill();
    }
}
