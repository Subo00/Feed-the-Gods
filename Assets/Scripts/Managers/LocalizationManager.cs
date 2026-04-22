using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }


    private const string CsvFileName = "localization.csv";
    private const string DefaultLanguage = "ENG";

    // key → (language → text)
    private Dictionary<string, Dictionary<string, string>> localizedLines;
    private string currentLanguage = DefaultLanguage;
    private List<string> availableLanguages = new List<string>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCSV();
    }

    public string GetLine(string key)
    {
        if (localizedLines == null || !localizedLines.TryGetValue(key, out var translations))
        {
            Debug.LogWarning($"[Localization] Key not found: {key}");
            return key;
        }

        if (translations.TryGetValue(currentLanguage, out var text) && !string.IsNullOrEmpty(text))
            return text;

        if (translations.TryGetValue(DefaultLanguage, out var fallback) && !string.IsNullOrEmpty(fallback))
        {
            Debug.LogWarning($"[Localization] Missing translation for '{key}' in '{currentLanguage}', falling back to {DefaultLanguage}.");
            return fallback;
        }

        Debug.LogWarning($"[Localization] No text found for key: {key}");
        return key;
    }

   
    public void SetLanguage(string language)
    {
        if (!availableLanguages.Contains(language))
        {
            Debug.LogWarning($"[Localization] Language '{language}' not found in CSV.");
            return;
        }

        currentLanguage = language;
    }

  
    public List<string> GetLines(string prefix)
    {
        var matches = new List<(int index, string text)>();

        foreach (var key in localizedLines.Keys)
        {
            if (!key.StartsWith(prefix + "_"))
                continue;

            string suffix = key.Substring(prefix.Length + 1);

            if (int.TryParse(suffix, out int index))
                matches.Add((index, GetLine(key)));
        }

        matches.Sort((a, b) => a.index.CompareTo(b.index));

        var result = new List<string>();
        foreach (var match in matches)
            result.Add(match.text);

        return result;
    }

    public string GetCurrentLanguage() => currentLanguage;
    public List<string> GetAvailableLanguages() => availableLanguages;

    private void LoadCSV()
    {
        localizedLines = new Dictionary<string, Dictionary<string, string>>();

        string path = Path.Combine(Application.streamingAssetsPath, CsvFileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"[Localization] CSV not found at: {path}");
            return;
        }

        string[] rows = File.ReadAllLines(path);

        if (rows.Length < 2)
        {
            Debug.LogError("[Localization] CSV has no data rows.");
            return;
        }

        // First row is the header: key, EN, PT, ...
        string[] headers = SplitCSVRow(rows[0]);

        for (int col = 1; col < headers.Length; col++)
        {
            string lang = headers[col].Trim();
            if (!availableLanguages.Contains(lang))
                availableLanguages.Add(lang);
        }

        // Remaining rows are data
        for (int row = 1; row < rows.Length; row++)
        {
            string[] columns = SplitCSVRow(rows[row]);

            if (columns.Length == 0 || string.IsNullOrWhiteSpace(columns[0]))
                continue;

            string key = columns[0].Trim();
            var translations = new Dictionary<string, string>();

            for (int col = 1; col < headers.Length && col < columns.Length; col++)
            {
                string lang = headers[col].Trim();
                translations[lang] = columns[col].Trim();
            }

            localizedLines[key] = translations;
        }

        Debug.Log($"[Localization] Loaded {localizedLines.Count} keys. Languages: {string.Join(", ", availableLanguages)}");
    }

    // Handles quoted CSV fields that may contain commas.
    private string[] SplitCSVRow(string row)
    {
        var fields = new List<string>();
        bool inQuotes = false;
        var current = new System.Text.StringBuilder();

        foreach (char c in row)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString());
        return fields.ToArray();
    }
}
