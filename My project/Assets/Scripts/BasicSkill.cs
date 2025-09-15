using UnityEngine;

public class BasicSkill : ISkill
{
    public void Init(SkillData data, PoolManager pool, Transform parent)
    {
        //none
    }

    public void Init(SkillData data, PoolManager pool, Transform parent, MonsterScanner scanner)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateSkill()
    {
        // none
    }
}
