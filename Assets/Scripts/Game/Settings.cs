using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private Locale[] languages;

    private void Start()
    {
        // Select game language
        LocalizationSettings.SelectedLocale = languages[PlayerPrefs.GetInt("selected_lang", 0)];
    }
}
