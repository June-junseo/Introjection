using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(SkillData data)
    {

        if (data.id == 10001)
        {
            return new DisasterSkill();
        }
        if(data.id == 10002)
        {
            return new CirculationSkill();
        }

        switch (data.skillGroup)
        {
            case "SWORD": return new LongSwordSkill();
            case "DAGGER": return new DaggerSkill();
            case "STAFF": return new StaffSkill();
            case "BOW": return new ArrowSkill();
            case "ROSARIO": return new RosarioSkill();
            default:
                Debug.LogWarning($"Unknown skill type: {data.skillGroup}");
                return null;
        }
    }

}
