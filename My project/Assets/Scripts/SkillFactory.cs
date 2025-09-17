using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(SkillData data)
    {
        switch (data.type)
        {
            case 1: // 롱소드
                return new LongSwordSkill();
            case 3: // 단검
                return new DaggerSkill();
            case 2: // 스태프
                return new StaffSkill();
            default:
                Debug.LogWarning($"Unknown skill type: {data.type}");
                return null;
        }
    }
}
