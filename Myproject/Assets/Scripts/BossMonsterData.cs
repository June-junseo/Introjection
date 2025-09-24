using UnityEngine;

[CreateAssetMenu(fileName = "EliteMonsterData", menuName = "Game/Elite Monster Data")]
public class EliteMonsterData : ScriptableObject, ICSVImportable
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
    public int drop1MaxCount;
    public int drop1MinCount;
    public string drop2;
    public int drop2MaxCount;
    public int drop2MinCount;

    public void ImportFromCSV(string[] cols)
    {
        id = int.Parse(cols[0]);
        monsterName = cols[1];
        type = (MonsterType)System.Enum.Parse(typeof(MonsterType), cols[2]);
        baseHp = float.Parse(cols[3]);
        baseAtk = float.Parse(cols[4]);
        speed = float.Parse(cols[5]);
        atkSpeed = float.Parse(cols[6]);

        drop1 = cols[7];
        drop1MaxCount = string.IsNullOrEmpty(cols[8]) ? 0 : int.Parse(cols[8]);
        drop1MinCount = string.IsNullOrEmpty(cols[9]) ? 0 : int.Parse(cols[9]);

        drop2 = cols[10];
        drop2MaxCount = string.IsNullOrEmpty(cols[11]) ? 0 : int.Parse(cols[11]);
        drop2MinCount = string.IsNullOrEmpty(cols[12]) ? 0 : int.Parse(cols[12]);

        if (cols.Length > 13 && !string.IsNullOrEmpty(cols[13]))
        {
            prefab = Resources.Load<GameObject>(cols[13]);
        }
    }
}
