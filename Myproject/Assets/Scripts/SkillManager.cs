using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skillDatas;  
    public PoolManager pool;

    private List<ISkill> skills = new List<ISkill>();

    private void Start()
    {
        SkillData starter = skillDatas.Find(s => s.level == 1 && s.skillGroup == "SWORD");
        if (starter != null)
            AddSkill(starter);
    }

    private void Update()
    {
        foreach (var s in skills)
            s.UpdateSkill();
    }

    public void AddSkill(SkillData newSkillData)
    {
        if (newSkillData == null)
        {
            return;
        }

        ISkill existingSkill = skills.Find(s => s != null && s.Data != null && s.Data.skillGroup == newSkillData.skillGroup);

        if (existingSkill != null)
        {

            existingSkill.Init(newSkillData, pool, transform);
            Debug.Log($"½ºÅ³ ·¹º§¾÷: {newSkillData.skillName} Lv.{newSkillData.level}");
            return;
        }


        if (newSkillData.level != 1) return;

        ISkill skill = SkillFactory.CreateSkill(newSkillData);
        skill.Init(newSkillData, pool, transform);
        skills.Add(skill);
        Debug.Log($"»õ ½ºÅ³ È¹µæ: {newSkillData.skillName} Lv.{newSkillData.level}");
    }

    public bool HasSkill(string skillGroup)
    {
        return skills.Exists(s => s.Data.skillGroup == skillGroup);
    }

    public SkillData GetCurrentSkill(string skillGroup)
    {
        return skills.Find(s => s.Data.skillGroup == skillGroup)?.Data;
    }

    public SkillData GetNextLevelSkill(SkillData current)
    {
        if (current == null)
        {
            return null;
        }

        return skillDatas.Find(s => s.skillGroup == current.skillGroup && s.level == current.level + 1);
    }

    public List<ISkill> GetAllSkills()
    {
        return skills;
    }
}
