using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System.Collections;
using UnityEngine;

public class StartScreenController : MonoBehaviour
{
    [SerializeField]
    private Locale[] languages;

    private void Start()
    {
        LocalizationSettings.SelectedLocale = languages[PlayerPrefs.GetInt("selected_lang", 0)];
    }
}
