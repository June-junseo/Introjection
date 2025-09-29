using System;
using UnityEngine;

[Serializable]
public class SkillEvolutionData : ICSVImportable
{
    public int id;
    public int skill_active_id;
    public int passive_id;
    public int evo_skill_id;
    public int unlock_condition;

    public void ImportFromCSV(string[] cols)
    {
        if (cols.Length < 5)
        {
            Debug.LogWarning("CSV 열 개수 부족!");
            return;
        }

        id = int.Parse(cols[0]);
        skill_active_id = int.Parse(cols[1]);
        passive_id = int.Parse(cols[2]);
        evo_skill_id = int.Parse(cols[3]);
        unlock_condition = int.Parse(cols[4]);
    }
}
