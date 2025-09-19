using UnityEngine;

public class BasicSkill : ISkill
{
    public SkillData Data => throw new System.NotImplementedException();

    public int CurrentLevel => throw new System.NotImplementedException();

    SkillData ISkill.Data { get => Data; set => throw new System.NotImplementedException(); }

    public void Init(SkillData data, PoolManager pool, Transform parent)
    {
        //none
    }

  

    public void UpdateSkill()
    {
        // none
    }
}
