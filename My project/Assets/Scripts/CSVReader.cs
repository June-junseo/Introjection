using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public static class CSVReader
{

    public static List<string[]> Read(string path)
    {
        List<string[]> result = new List<string[]>();
        string[] lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // "로 감싸진 값 처리
            List<string> cols = new List<string>();
            foreach (Match m in Regex.Matches(line, @"(?:^|,)(?:""(?<val>[^""]*)""|(?<val>[^,]*))"))
            {
                cols.Add(m.Groups["val"].Value);
            }

            result.Add(cols.ToArray());
        }

        return result;
    }
}
