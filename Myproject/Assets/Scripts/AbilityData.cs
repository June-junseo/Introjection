using UnityEngine;
using System;

[Serializable]
public class AbilityData : ICSVImportable
{
    public int id;
    public string abilityName;
    public string abilityGroup;
    public int affectPassive;
    public float abilityValue;
    public int statLevel;
    public int baseCost;
    public float costGrowth;
    public string unlockCondition;
    public string iconImage;
    public string description;

    public void ImportFromCSV(string[] row)
    {
        id = int.Parse(row[0]);
        abilityName = row[1];
        abilityGroup = row[2];
        affectPassive = int.Parse(row[3]);
        abilityValue = float.Parse(row[4]);
        statLevel = int.Parse(row[5]);
        baseCost = int.Parse(row[6]); 
        costGrowth = float.Parse(row[7]);
        unlockCondition = row[8];
        iconImage = row[9];
        description = row[10];
    }
}
