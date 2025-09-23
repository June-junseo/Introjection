using UnityEngine;
using System;

[Serializable]
public class PassiveSkillData : ICSVImportable
{
    public int id;
    public string skillName;
    public string passiveGroup;
    public int craftCode;
    public int affectAbility;
    public float passiveValue;
    public string description;
    public string iconPath;
    public string unlockCondition;
    public int level;
    public string flavorText;

    public void ImportFromCSV(string[] row)
    {
        id = int.TryParse(row[0].Trim(), out int tmpId) ? tmpId : 0;
        skillName = row[1].Trim();
        passiveGroup = row[2].Trim();
        craftCode = int.TryParse(row[3].Trim(), out int tmpCraft) ? tmpCraft : 0;
        affectAbility = int.TryParse(row[4].Trim(), out int tmpAff) ? tmpAff : 0;
        passiveValue = float.TryParse(row[5].Trim(), out float tmpVal) ? tmpVal : 0f;
        description = row[6].Trim();
        iconPath = row[7].Trim();
        unlockCondition = row[8].Trim();
        level = int.TryParse(row[9].Trim(), out int tmpLevel) ? tmpLevel : 1;
        flavorText = row[10].Trim();
    }
}
