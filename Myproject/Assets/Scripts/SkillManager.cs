using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skillDatas;
    public PoolManager pool;

    private List<ISkill> skills = new List<ISkill>();

    private void Start()
    {
        SkillData starter = skillDatas.Find(s => s.level == 1 && s.skillName.Contains("·Õ¼Òµå"));
        if (starter != null)
        {
            AddSkill(starter);
        }
    }

    private void Update()
    {
        foreach (var s in skills)
        {
            s.UpdateSkill();
        }
    }

    public void AddSkill(SkillData newSkillData)
    {
        if (newSkillData == null)
        {
            return;
        }

        ISkill existing = skills.Find(s => s != null && s.Data != null && s.Data.skillName == newSkillData.skillName);

        if (existing != null)
        {
            SkillData nextLevel = GetNextLevelSkill(existing.Data);
            if (nextLevel != null && nextLevel.id == newSkillData.id)
            {
                existing.Init(nextLevel, pool, transform);
                Debug.Log($"½ºÅ³ ·¹º§¾÷: {nextLevel.skillName} Lv.{nextLevel.level}");
            }
            return;
        }

        if (newSkillData.level != 1)
        {
            return;
        }

        ISkill skill = SkillFactory.CreateSkill(newSkillData);
        skill.Init(newSkillData, pool, transform);
        skills.Add(skill);

        Debug.Log($"»õ ½ºÅ³ È¹µæ: {newSkillData.skillName} Lv.{newSkillData.level}");
    }

    public bool HasSkill(string skillName)
    {
        return skills.Exists(s => s != null && s.Data != null && s.Data.skillName == skillName);
    }

    public SkillData GetCurrentSkill(string skillName)
    {
        var skill = skills.Find(s => s != null && s.Data != null && s.Data.skillName == skillName);
        return skill?.Data;
    }

    public SkillData GetNextLevelSkill(SkillData current)
    {
        if (current == null)
        {
            return null;
        }
        return skillDatas.Find(s => s.skillName == current.skillName && s.level == current.level + 1);
    }

    public List<ISkill> GetAllSkills()
    {
        return skills;
    }
}
