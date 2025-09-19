using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(SkillData data)
    {
        switch (data.id)
        {
            case 1170111101: // �ռҵ�
                return new LongSwordSkill();
            case 1200211121: // �ܰ�
                return new DaggerSkill();
            case 1125211106: // ������
                return new StaffSkill();
            default:
                Debug.LogWarning($"Unknown skill type: {data.type}");
                return null;
        }
    }
}
