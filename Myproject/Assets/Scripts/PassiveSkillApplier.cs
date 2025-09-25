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
        }
    }
}

