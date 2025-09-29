using UnityEngine;
using System.Diagnostics;

public class PassiveSkillApplier
{
    private CharacterStats stats;

    public PassiveSkillApplier(CharacterStats stats)
    {
        this.stats = stats;
    }

    public void ApplyAll(PassiveSkillData[] passives)
    {
        stats.ResetStats();

        foreach (var passive in passives)
        {
            Apply(passive);
        }
    }


    private void Apply(PassiveSkillData passive)
    {
        switch (passive.affectAbility)
        {
            case 1:
                stats.attackMultiplier += passive.passiveValue;
                break;
            case 2: 
                stats.cooldownReduction += passive.passiveValue;
                break;
            case 3: 
                stats.moveSpeed += passive.passiveValue;
                break;
            case 4: 
                stats.critRate += passive.passiveValue;
                break;
            case 5: 
                stats.defenseRate += passive.passiveValue;
                break;
            case 6: 
                stats.projectileBonus += Mathf.RoundToInt(passive.passiveValue);
                break;
        }
    }

}

