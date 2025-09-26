using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skillDatas;
    public TextAsset passiveCSV;
    public PoolManager pool;
    public CharacterStats stats;

    private List<ISkill> skills = new List<ISkill>();
    private List<PassiveSkillData> passiveSkills = new List<PassiveSkillData>();
    private PassiveSkillApplier applier;

    public List<PassiveSkillData> PassiveSkills => passiveSkills;

    private void Awake()
    {
        applier = new PassiveSkillApplier(stats);
    }

    private void Start()
    {
        foreach (var data in skillDatas)
        {
            if (data.level == 1 && data.skillGroup == "SWORD") 
            {
                AddSkill(data);
                break;
            }
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

        ISkill existing = skills.Find(s => s.Data != null && s.Data.skillGroup == newSkillData.skillGroup);

        if (existing != null)
        {
            existing.Init(newSkillData, pool, transform, stats);
            Debug.Log($"스킬 레벨업: {newSkillData.skillName} Lv.{newSkillData.level}");
            return;
        }

        if (newSkillData.level != 1)
        {
            return;
        }

        ISkill skill = SkillFactory.CreateSkill(newSkillData);
        skill.Init(newSkillData, pool, transform, stats);
        skills.Add(skill);
        Debug.Log($"새 스킬 획득: {newSkillData.skillName} Lv.{newSkillData.level}");
    }

    public void AddPassiveSkill(PassiveSkillData newPassive)
    {
        if (newPassive == null)
        {
            return;
        }

        PassiveSkillData existing = passiveSkills.Find(p => p.passiveGroup == newPassive.passiveGroup);

        if (existing != null)
        {
            int levelDiff = newPassive.level - existing.level;
            if (levelDiff > 0)
            {
                passiveSkills.Remove(existing);
                passiveSkills.Add(newPassive);
                Debug.Log($"패시브 레벨업: {newPassive.skillName} Lv.{newPassive.level}");
            }
            applier.ApplyAll(passiveSkills.ToArray());
            return;
        }

        if (newPassive.level == 1)
        {
            passiveSkills.Add(newPassive);
            applier.ApplyAll(passiveSkills.ToArray());
            Debug.Log($"새 패시브 획득: {newPassive.skillName} Lv.{newPassive.level}");
        }
    }


    public SkillData GetCurrentSkill(string skillGroup) =>
        skills.Find(s => s.Data.skillGroup == skillGroup)?.Data;

    public SkillData GetNextLevelSkill(SkillData current) =>
        skillDatas.Find(s => s.skillGroup == current.skillGroup && s.level == current.level + 1);

    public PassiveSkillData GetCurrentPassive(string passiveGroup) =>
        passiveSkills.Find(p => p.passiveGroup == passiveGroup);

    public PassiveSkillData GetNextLevelPassive(PassiveSkillData current)
    {
        return CSVLoader.LoadCSV<PassiveSkillData>(passiveCSV)
            .Find(p => p.passiveGroup == current.passiveGroup && p.level == current.level + 1);
    }

    public List<ISkill> GetOwnedActiveSkills()
    {
        return skills; 
    }

    public bool CanAddActiveSkill()
    {
        return skills.Count < 3;
    }

    public bool CanAddPassiveSkill()
    {
        return passiveSkills.Count < 5;
    }

}
