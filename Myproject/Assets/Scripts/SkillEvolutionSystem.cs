using System.Collections.Generic;
using UnityEngine;

public class SkillEvolutionSystem
{
    private readonly Dictionary<(int, int), SkillEvolutionData> map = new();

    public SkillEvolutionSystem(IEnumerable<SkillEvolutionData> datas)
    {
        if (datas == null)
        {
            return;
        }

        foreach (var d in datas)
        {
            if (d == null)
            {
                continue;
            }
            map[(d.skill_active_id, d.passive_id)] = d;
        }
    }

    public bool TryGetEvolution(int activeId, int passiveId, out SkillEvolutionData evoData)
    {
        return map.TryGetValue((activeId, passiveId), out evoData);
    }
}
