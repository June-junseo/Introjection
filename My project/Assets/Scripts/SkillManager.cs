using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skillDatas;
    public PoolManager pool;
    //public MonsterScanner scanner;

    private List<ISkill> skills = new List<ISkill>();
    private ISkill basicSkill;

    private void Start()
    {
        if (skillDatas == null || skillDatas.Count == 0 || pool == null)
            return;

        ISkill firstSkill = SkillFactory.CreateSkill(skillDatas[0]);
        firstSkill.Init(skillDatas[0], pool, transform);
        skills.Add(firstSkill);
        basicSkill = firstSkill;
    }

    private void Update()
    {
        if(basicSkill != null)
        {
            basicSkill.UpdateSkill();
        }

        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].UpdateSkill();
        }
    }

    public void AddSkills(SkillData newSkillData)
    {
        ISkill newSkill = SkillFactory.CreateSkill(newSkillData);

        newSkill.Init(newSkillData, pool, transform);
        skills.Add(newSkill);

        Debug.Log("½ºÅ³ Ãß°¡µÊ");
    }

    
}
