using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(SkillData data)
    {
        switch (data.skillGroup)
        {
            case "SWORD": // �ռҵ�
                return new LongSwordSkill();
            case "DAGGER": // �ܰ�
                return new DaggerSkill();
            case "STAFF": // ������
                return new StaffSkill();
            default:
                Debug.LogWarning($"Unknown skill type: {data.skillGroup}");
                return null;
        }
    }
}
