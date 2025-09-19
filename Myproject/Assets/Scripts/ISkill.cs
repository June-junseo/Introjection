using UnityEngine;

public interface ISkill
{
    SkillData Data { get; set; }
    void Init(SkillData data, PoolManager pool, Transform parent);
    void UpdateSkill();
}
