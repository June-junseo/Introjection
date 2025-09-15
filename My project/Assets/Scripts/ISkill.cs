using UnityEngine;

public interface ISkill
{
    void Init(SkillData data, PoolManager pool, Transform parent);
    void Init(SkillData data, PoolManager pool, Transform parent, MonsterScanner scanner);
    void UpdateSkill();
}
