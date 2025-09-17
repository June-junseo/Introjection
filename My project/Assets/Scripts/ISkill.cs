using UnityEngine;

public interface ISkill
{
    void Init(SkillData data, PoolManager pool, Transform parent);
    void UpdateSkill();
}
