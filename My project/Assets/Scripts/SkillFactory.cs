using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(SkillData data)
    {
        switch (data.type)
        {
            case 1: // �ռҵ�
                return new LongSwordSkill();
            case 3: // �ܰ�
                return new DaggerSkill();
            case 2: // ������
                return new StaffSkill();
            default:
                Debug.LogWarning($"Unknown skill type: {data.type}");
                return null;
        }
    }
}
