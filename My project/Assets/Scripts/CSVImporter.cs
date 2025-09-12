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
            Debug.LogError("CSV ���ϰ� ���ø� SO�� �����ϼ���.");
            return;
        }

        List<string[]> rows = CSVReader.Read(AssetDatabase.GetAssetPath(csvFile));
        if (rows.Count < 2) return; // �ּ� 2�� �̻� (��� + ������)

        for (int i = 1; i < rows.Count; i++)
        {
            ScriptableObject obj = ScriptableObject.CreateInstance(template.GetType());
            (obj as ICSVImportable)?.ImportFromCSV(rows[i]);

            string assetPath = $"{assetFolder}/{rows[i][0]}.asset";
            AssetDatabase.CreateAsset(obj, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"{template.GetType().Name} CSV ����Ʈ �Ϸ�!");
    }
}
