using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SkillDataFile", menuName = "Game/Skill Data")]
public class SkillData : ScriptableObject, ICSVImportable
{
    public int id;
    public string skillName;
    public int skillCategory;
    public float cooltime;
    public float duration;
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

    public GameObject skillPrefab;

    public void ImportFromCSV(string[] cols)
    {
        id = ToInt(cols[0]);
        skillName = cols[1];
        skillCategory = ToInt(cols[2]);
        cooltime = ToFloat(cols[3]);
        duration = ToFloat(cols[4]);
        type = ToInt(cols[5]);
        isTickDamage = ToBool(cols[6]);
        tickInterval = ToFloat(cols[7]);
        hasProjectile = ToBool(cols[8]);
        canCritical = ToBool(cols[9]);
        damagePercent = ToFloat(cols[10]);
        range = ToFloat(cols[11]);
        level = ToInt(cols[12]);
        projectileCount = ToInt(cols[13]);
        criticalRate = ToFloat(cols[14]);
        statusEffect = ToInt(cols[15]);
        unlockCondition = cols[16];
        iconPath = cols[17];
        description = cols.Length > 19 ? cols[18] : "";

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
