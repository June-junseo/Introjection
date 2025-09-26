using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CSVImporter : EditorWindow
{
    [MenuItem("Tools/Import CSV/Generic Importer")]
    public static void ShowWindow()
    {
        GetWindow<CSVImporter>("CSV Importer");
    }

    private TextAsset csvFile;
    private ScriptableObject template;
    private string assetFolder = "Assets/Data";

    void OnGUI()
    {
        GUILayout.Label("CSV Importer", EditorStyles.boldLabel);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);
        template = (ScriptableObject)EditorGUILayout.ObjectField("Template SO", template, typeof(ScriptableObject), false);
        assetFolder = EditorGUILayout.TextField("Output Folder", assetFolder);

        if (GUILayout.Button("Import"))
        {
            Import();
        }
    }

    private void Import()
    {
        if (csvFile == null || template == null)
        {
            Debug.LogError("CSV 파일과 템플릿 SO를 설정하세요.");
            return;
        }

        List<string[]> rows = CSVReader.Read(AssetDatabase.GetAssetPath(csvFile));
        if (rows.Count < 2)
        {
            Debug.LogWarning("CSV 데이터가 없습니다.");
            return;
        }

        int createdCount = 0;

        for (int i = 1; i < rows.Count; i++)
        {
            var row = rows[i];

            if (row.Length < 2)
            {
                Debug.LogWarning($"CSV {i + 1}행: 컬럼이 부족합니다. 건너뜁니다.");
                continue;
            }

            for (int j = 0; j < row.Length; j++)
            {
                if (string.IsNullOrEmpty(row[j]))
                {
                    row[j] = string.Empty;
                }
            }

            ScriptableObject obj = ScriptableObject.CreateInstance(template.GetType());
            (obj as ICSVImportable)?.ImportFromCSV(row);

            string safeName = MakeSafeAssetName(row[1]);
            string assetPath = $"{assetFolder}/{safeName}.asset";

            AssetDatabase.CreateAsset(obj, assetPath);
            createdCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"{template.GetType().Name} CSV 임포트 완료됨, 생성된 에셋 count: {createdCount}");
    }

    private string MakeSafeAssetName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return "Unnamed";
        }

        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }

        name = name.Replace(' ', '_');
        name = name.Trim('_');

        return name;
    }
}
