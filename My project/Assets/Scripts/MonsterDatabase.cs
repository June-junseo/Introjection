using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDatabase", menuName = "Game/Monster Database", order = 10)]
public class MonsterDatabase : ScriptableObject
{
    public MonsterData[] monsters;

    public MonsterData GetData(MonsterType type)
    {
        foreach (var m in monsters)
        {
            if (m.type == type) return m;
        }
        return null;
    }
}
