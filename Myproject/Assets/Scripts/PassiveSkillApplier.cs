using System.Diagnostics;

public class PassiveSkillApplier
{
    private CharacterStats stats;

    public PassiveSkillApplier(CharacterStats stats)
    {
        this.stats = stats;
    }

    public void Apply(PassiveSkillData passive)
    {
        switch (passive.affectAbility)
        {
            case 0: 
                stats.attackMultiplier += passive.passiveValue;
                break;
            case 1: 
                stats.cooldownReduction += passive.passiveValue;
                break;
            case 2: 
                stats.moveSpeed += passive.passiveValue;
                break;
            default:
                //Debug.Log($"Unknown affectAbility {passive.affectAbility}");
                break;
        }
    }
}
