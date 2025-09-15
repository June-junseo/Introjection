using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skillDatas;
    public PoolManager pool;
    public MonsterScanner scanner;

    private List<ISkill> skills = new List<ISkill>();

    private void Start()
    {
        if (skillDatas == null || pool == null)
            return;

        foreach (var skillData in skillDatas)
        {
            ISkill skill = SkillFactory.CreateSkill(skillData);
            if (skill == null) continue;

            if (skill is StaffSkill staff)
                staff.Init(skillData, pool, transform, scanner);
            else
                skill.Init(skillData, pool, transform);

            skills.Add(skill);
        }
    }

    private void Update()
    {
        foreach (var skill in skills)
        {
            skill.UpdateSkill();
        }
    }
}
