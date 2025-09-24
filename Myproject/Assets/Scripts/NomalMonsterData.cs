using UnityEngine;

public enum MonsterType
{
    Normal_Slime1,
    Normal_Slime2,
    Normal_Skeleton1,
    Normal_Skeleton2,
    Normal_Orc1,
    Normal_Orc2,
    Elite_Orc,
    Elite_Slime,
    Elite_Skeleton,
    Boss_Dragon,
    Boss_Warrior
}

[CreateAssetMenu(fileName = "NormalMonsterData", menuName = "Game/Normal Monster Data")]
public class NormalMonsterData : ScriptableObject, ICSVImportable
{
    public int id;
    public MonsterType type;
    public string monsterName;
    public float baseHp;
    public float baseAtk;
    public float speed;
    public float atkSpeed;

    public GameObject prefab;

    public string drop1;
    public float drop1Rate;
    public string drop2;
    public float drop2Rate;

    public void ImportFromCSV(string[] cols)
    {
        id = int.Parse(cols[0]);
        monsterName = cols[1];
        baseHp = float.Parse(cols[2]);
        baseAtk = float.Parse(cols[3]);
        speed = float.Parse(cols[4]);
        atkSpeed = float.Parse(cols[5]);
        drop1 = cols[6];
        drop1Rate = float.Parse(cols[7]);
        drop2 = cols[8];
        drop2Rate = string.IsNullOrEmpty(cols[9]) ? 0f : float.Parse(cols[9]);

        if (cols.Length > 10 && !string.IsNullOrEmpty(cols[10]))
        {
            prefab = Resources.Load<GameObject>(cols[10]);
        }

        if (cols.Length > 11)
        {
            type = (MonsterType)System.Enum.Parse(typeof(MonsterType), cols[11]);
        }
    }
}
