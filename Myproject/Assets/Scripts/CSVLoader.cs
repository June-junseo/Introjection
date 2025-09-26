using System.Collections.Generic;
using UnityEngine;

public static class CSVLoader
{
    public static List<T> LoadCSV<T>(TextAsset csv) where T : ICSVImportable, new()
    {
        var rows = CSVReader.ReadFromTextAsset(csv);
        var result = new List<T>();

        if (rows.Count < 2)
        {
            return result;
        }

        for (int i = 1; i < rows.Count; i++)
        {
            var row = rows[i];
            if (row.Length == 0)
            {
                continue;
            }

            T obj = new T();
            obj.ImportFromCSV(row);
            result.Add(obj);
        }

        return result;
    }
}
