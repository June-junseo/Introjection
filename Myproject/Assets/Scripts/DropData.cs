using System.Globalization;
using UnityEngine;

[System.Serializable]
public class DropData : ICSVImportable
{
    public int ID;
    public int STAGE;
    public int DROP1;
    public float DROP1_RATE;
    public int DROP1_MaxCount;
    public int DROP2_MinCount;
    public int DROP2;
    public float DROP2_RATE;
    public int DROP2_COUNT;

    public void ImportFromCSV(string[] row)
    {
        if (row.Length < 9)
        {
            Debug.LogError("CSV row does not have enough columns!");
            return;
        }

        ID = int.Parse(row[0]);
        STAGE = int.Parse(row[1]);
        DROP1 = int.Parse(row[2]);
        DROP1_RATE = float.Parse(row[3], CultureInfo.InvariantCulture);
        DROP1_MaxCount = int.Parse(row[4]);
        DROP2_MinCount = int.Parse(row[5]);
        DROP2 = int.Parse(row[6]);
        DROP2_RATE = float.Parse(row[7], CultureInfo.InvariantCulture);
        DROP2_COUNT = int.Parse(row[8]);
    }
}
