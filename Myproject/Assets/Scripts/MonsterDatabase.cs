using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDatabase", menuName = "Game/Monster Database")]
public class MonsterDatabase : ScriptableObject
{
    public NormalMonsterData[] normalMonsters;
    public EliteMonsterData[] eliteMonsters;

    public NormalMonsterData GetNormal(MonsterType type)
    {
        foreach (var m in normalMonsters)
        {
            if (m.type == type)
            {
                return m;
            }
        }
        return null;
    }

    public EliteMonsterData GetElite(MonsterType type)
    {
        foreach (var m in eliteMonsters)
        {
            if (m.type == type)
            {
                return m;
            }
        }
        return null;
    }
}
