using UnityEngine.UI;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    private Text text;
    private string key;

    private void Start()
    {
        Localize();

        // Add event on language change
        LocalizationController.OnLanguageChange += OnLanguageChange;
    }

    private void OnLanguageChange()
    {
        Localize();
    }

    private void Init()
    {
        text = GetComponent<Text>();
        key = text.text;
    }

    private void OnDestroy()
    {
        LocalizationController.OnLanguageChange -= OnLanguageChange;
    }

    public void Localize(string localizeKey = null)
    {
        if (text == null) {
            Init();
        }

        if (localizeKey != null) {
            key = localizeKey;
        }

        // Get the translation if it is founded
        text.text = LocalizationController.GetTranslation(key);
    }
}
