using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LocalizationController : MonoBehaviour
{
    private static int selectedLanguage;
    private static Dictionary<string, List<string>> localizationDict;

    public static event LanguageChange OnLanguageChange;
    public delegate void LanguageChange();

    public static int SelectedLanguage { get => selectedLanguage; }

    [SerializeField]
    private TextAsset textFile;

    private void Awake()
    {
        if (localizationDict == null) {
            LoadLocalizationDict();
        }

        SetLanguage(PlayerPrefs.GetInt("selected_lang", 0)); // English (0) by default
    }

    private void LoadLocalizationDict()
    {
        localizationDict = new Dictionary<string, List<string>>();

        // Get xml file data
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textFile.text);

        // Get localization keys and values
        foreach (XmlNode keyNode in xmlDocument["Keys"].ChildNodes) {
            string key = keyNode.Attributes["Name"].Value;

            List<string> translations = new List<string>();

            // Get each translation by key
            foreach (XmlNode translation in keyNode["Translates"].ChildNodes) {
                translations.Add(translation.InnerText);
            }

            // Save translations by key in dictionary
            localizationDict[key] = translations;
        }
    }

    public void SetLanguage(int index)
    {
        selectedLanguage = index;
        PlayerPrefs.SetInt("selected_lang", index);
        OnLanguageChange?.Invoke();
    }

    public static string GetTranslation(string key, int lang = -1)
    {
        if (lang == -1) {
            lang = SelectedLanguage;
        }

        // If dictonary has given key, get translation
        if (localizationDict.ContainsKey(key)) {
            return localizationDict[key][lang];
        }

        return key;
    }
}
