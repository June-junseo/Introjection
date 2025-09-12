using System.Collections.Generic;
using System.IO;

public static class CSVReader
{
    public static List<string[]> Read(string path)
    {
        List<string[]> result = new List<string[]>();
        string[] lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            result.Add(line.Split(','));
        }

        return result;
    }
}
