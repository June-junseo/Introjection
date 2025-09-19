using UnityEngine;

public enum MonsterType
{
    Monster1,
    Monster2,
}

public enum AIPattern
{
    Chase,
    RangeAttack
}


[CreateAssetMenu(fileName = "MonsterData", menuName = "Game/Monster Data", order = 1)]
public class MonsterData : ScriptableObject, ICSVImportable
{
    public MonsterType type;
    public GameObject prefab;
    public float maxHp;
    public float speed;
    public int damage;
    public int poolSize;
    public AIPattern aiPattern;

    public void ImportFromCSV(string[] cols)
    {
        name = cols[0];
        type = (MonsterType)System.Enum.Parse(typeof(MonsterType), cols[1]);
        maxHp = float.Parse(cols[2]);
        speed = float.Parse(cols[3]);
        damage = int.Parse(cols[4]);
        poolSize = int.Parse(cols[5]);
        prefab = Resources.Load<GameObject>(cols[6]);
        aiPattern = (AIPattern)System.Enum.Parse(typeof(AIPattern), cols[7]);
    }
}
