using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(SkillData data)
    {
        switch (data.skillGroup)
        {
            case "SWORD": // 롱소드
                return new LongSwordSkill();
            case "DAGGER": // 단검
                return new DaggerSkill();
            case "STAFF": // 스태프
                return new StaffSkill();
            case "BOW": 
                return new ArrowSkill();
            default:
                Debug.LogWarning($"Unknown skill type: {data.skillGroup}");
                return null;
        }
    }
}
