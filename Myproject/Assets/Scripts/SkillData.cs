using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SkillDatasFile", menuName = "Game/Skill Data")]
public class SkillData : ScriptableObject, ICSVImportable
{
    public int id;
    public string skillName;
    public int skillCategory;
    public float cooltime;
    public float duration;
    public string skillGroup;
    public int type;
    public bool isTickDamage;
    public float tickInterval;
    public bool hasProjectile;
    public bool canCritical;
    public float damagePercent;
    public float range;
    public int level;
    public int projectileCount;
    public float criticalRate;
    public int statusEffect;
    public string unlockCondition;
    public string iconPath;
    public string description;

    public void ImportFromCSV(string[] cols)
    {
        id = ToInt(cols[0]);
        skillName = cols[1];
        skillCategory = ToInt(cols[2]);
        cooltime = ToFloat(cols[3]);
        duration = ToFloat(cols[4]);
        skillGroup = cols[5];
        type = ToInt(cols[6]);
        isTickDamage = ToBool(cols[7]);
        tickInterval = ToFloat(cols[8]);
        hasProjectile = ToBool(cols[9]);
        canCritical = ToBool(cols[10]);
        damagePercent = ToFloat(cols[11]);
        range = ToFloat(cols[12]);
        level = ToInt(cols[13]);
        projectileCount = ToInt(cols[14]);
        criticalRate = ToFloat(cols[15]);
        statusEffect = ToInt(cols[16]);
        unlockCondition = cols[17];
        iconPath = cols[18];
        description = cols.Length > 20 ? cols[19] : "";

    }
  
    private int ToInt(string s, int defaultValue = 0)
    {
        if (string.IsNullOrWhiteSpace(s)) return defaultValue;
        if (int.TryParse(s, out int v)) return v;
        if (float.TryParse(s, out float f)) return Mathf.RoundToInt(f);
        return defaultValue;
    }

    private float ToFloat(string s, float defaultValue = 0f)
    {
        if (string.IsNullOrWhiteSpace(s)) return defaultValue;
        if (float.TryParse(s, out float v)) return v;
        return defaultValue;
    }

    private bool ToBool(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;
        s = s.Trim().ToLower();
        return s == "1" || s == "true" || s == "yes";
    }
}
