using System.Collections.Generic;
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

        passiveSkills = new List<PassiveSkillData>();
        applier = new PassiveSkillApplier(stats);
    }

    private void Start()
    {
        SkillData starter = skillDatas.Find(s => s.level == 1 && s.skillGroup == "SWORD");
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

        ISkill existingSkill = skills.Find(s => s.Data != null && s.Data.skillGroup == newSkillData.skillGroup);

        if (existingSkill != null)
        {
            existingSkill.Init(newSkillData, pool, transform, stats);
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
            passiveSkills.Remove(existing);
            passiveSkills.Add(newPassive);
            applier.Apply(newPassive);
            Debug.Log($"패시브 레벨업: {newPassive.skillName} Lv.{newPassive.level}");
            return;
        }

        if (newPassive.level != 1)
        {
            return;
        }

        passiveSkills.Add(newPassive);
        applier.Apply(newPassive);
        Debug.Log($"새 패시브 획득: {newPassive.skillName} Lv.{newPassive.level}");
    }

    public SkillData GetCurrentSkill(string skillGroup) => skills.Find(s => s.Data.skillGroup == skillGroup)?.Data;

    public SkillData GetNextLevelSkill(SkillData current) =>
        skillDatas.Find(s => s.skillGroup == current.skillGroup && s.level == current.level + 1);

    public PassiveSkillData GetCurrentPassive(string passiveGroup) =>
        passiveSkills.Find(p => p.passiveGroup == passiveGroup);

    public PassiveSkillData GetNextLevelPassive(PassiveSkillData current) =>
        passiveSkills.Find(s => s.passiveGroup == current.passiveGroup && s.level == current.level + 1);
}
