using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skillDatas;
    public TextAsset passiveCSV;
    public TextAsset evolutionCSV;

    public PoolManager pool;
    public CharacterStats stats;
    public SelectSkillUi selectSkillUi;
    public OwnedSkillUi ownedSkillUi;

    private List<ISkill> skills = new List<ISkill>();
    private List<PassiveSkillData> passiveSkills = new List<PassiveSkillData>();
    private PassiveSkillApplier applier;
    public SkillEvolutionSystem evolutionSystem;

    public List<PassiveSkillData> PassiveSkills => passiveSkills;

    public HashSet<string> evolvedSkillGroups = new HashSet<string>();

    public void MarkEvolved(string skillGroup)
    {
        evolvedSkillGroups.Add(skillGroup);
    }

    private void Awake()
    {
        applier = new PassiveSkillApplier(stats);

        if (evolutionCSV != null)
        {
            var evoList = CSVLoader.LoadCSV<SkillEvolutionData>(evolutionCSV);
            evolutionSystem = new SkillEvolutionSystem(evoList);
        }
        else
        {
            evolutionSystem = new SkillEvolutionSystem(null);
        }
    }

    public void InitSkillManager()
    {
        foreach (var data in skillDatas)
        {
            if (data.level == 1 && data.skillGroup == "SWORD")
            {
                ReplaceOrAddSkillByGroup(data.skillGroup, data);
                break;
            }
        }

        ownedSkillUi?.RefreshOwnedSkills();
    }

    private void Start()
    {
        foreach (var data in skillDatas)
        {
            if (data.level == 1 && data.skillGroup == "SWORD")
            {
                ReplaceOrAddSkillByGroup(data.skillGroup, data);
                break;
            }
        }

        ownedSkillUi?.RefreshOwnedSkills();
    }

    private void Update()
    {
        foreach (var s in skills)
        {
            s.UpdateSkill();
        }
    }

    public void ReplaceOrAddSkillByGroup(string skillGroup, SkillData newData)
    {
        if (newData == null)
        {
            return;
        }

        var existingSkill = skills.Find(s => s.Data.skillGroup == skillGroup);

        if (existingSkill != null)
        {
            existingSkill.ResetSkill();
            skills.Remove(existingSkill);
        }

        ISkill newSkill = SkillFactory.CreateSkill(newData);
        newSkill.Init(newData, pool, transform, stats);
        skills.Add(newSkill);

        if (ownedSkillUi != null)
        {
            var slot = ownedSkillUi.activeSkillSlots.FirstOrDefault(s => s.skillGroup == skillGroup);
            Sprite icon = Resources.Load<Sprite>(newData.iconPath) ?? Resources.Load<Sprite>("Icons/DefaultIcon");

            if (slot != null)
            {
                slot.skillGroup = newData.skillGroup;
                slot.icon.sprite = icon;
                slot.icon.gameObject.SetActive(true);
                slot.icon.SetAllDirty();
            }
            else
            {
                ownedSkillUi.RefreshOwnedSkills();
            }
        }

        Debug.Log($"[SkillManager] 스킬 교체/추가 완료: {skillGroup} → {newData.skillName}");
    }

    public void AddSkill(SkillData newSkillData)
    {
        if (newSkillData == null)
        {
            return;
        }

        var existingSkill = skills.Find(s => s.Data.skillGroup == newSkillData.skillGroup);

        if (existingSkill != null)
        {
            ReplaceOrAddSkillByGroup(existingSkill.Data.skillGroup, newSkillData);
            CheckEvolutionForSingleActive(existingSkill);
            return;
        }

        if (newSkillData.level != 1)
        {
            return;
        }

        ReplaceOrAddSkillByGroup(newSkillData.skillGroup, newSkillData);
        CheckEvolutionForSingleActive(skills.Last());
    }

    public void AddPassiveSkill(PassiveSkillData newPassive)
    {
        if (newPassive == null)
        {
            return;
        }

        var existing = passiveSkills.Find(p => p.passiveGroup == newPassive.passiveGroup);

        if (existing != null && newPassive.level > existing.level)
        {
            passiveSkills.Remove(existing);
            passiveSkills.Add(newPassive);
        }
        else if (existing == null && newPassive.level == 1)
        {
            passiveSkills.Add(newPassive);
        }
        else
        {
            return; 
        }

        applier.ApplyAll(passiveSkills.ToArray());
        CheckAllEvolutions();
        ownedSkillUi?.RefreshOwnedSkills();
    }

    private void CheckAllEvolutions()
    {
        foreach (var act in skills.ToArray())
        {
            CheckEvolutionForSingleActive(act);
        }
    }

    public void CheckEvolutionForSingleActive(ISkill activeSkill, bool allowAutomatic = false)
    {
        if (activeSkill == null || activeSkill.Data == null || evolutionSystem == null)
        {
            return;
        }

        List<SkillEvolutionData> evoOptions = new List<SkillEvolutionData>();

        foreach (var passive in passiveSkills)
        {
            if (passive == null)
            {
                continue;
            }

            if (evolutionSystem.TryGetEvolution(activeSkill.Data.id, passive.id, out var evoData))
            {
                if (evoData.unlock_condition != 0)
                {
                    continue;
                }

                var evoSkillData = skillDatas.Find(s => s.id == evoData.evo_skill_id);
                if (evoSkillData == null)
                {
                    continue;
                }

                evoOptions.Add(evoData);
            }
        }

        if (evoOptions.Count > 0 && !allowAutomatic)
        {
            selectSkillUi?.OpenEvolutionUI(activeSkill.Data, evoOptions);
        }
    }


    public void ApplyEvolution(ISkill consumedActive, PassiveSkillData consumedPassive, SkillEvolutionData evoData)
    {
        if (consumedActive == null || evoData == null)
        {
            return;
        }

        var evoSkillData = skillDatas.Find(s => s.id == evoData.evo_skill_id);
        if (evoSkillData == null)
        {
            return;
        }

        evoSkillData.IsEvolutionSkill = true;

        foreach (var d in skillDatas.Where(s => s.skillGroup == consumedActive.Data.skillGroup))
        {
            d.IsConsumedForEvolution = true;
        }

        ReplaceOrAddSkillByGroup(consumedActive.Data.skillGroup, evoSkillData);
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

    public List<ISkill> GetOwnedActiveSkills() => skills;

    public bool CanAddActiveSkill() => skills.Count < 3;

    public bool CanAddPassiveSkill() => passiveSkills.Count < 5;
}
