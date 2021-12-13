using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class StartScreenController : MonoBehaviour
{
    [SerializeField]
    private float delayBeforeNextScene;
    [SerializeField]
    private int nextScene;
    [SerializeField]
    private Locale[] languages;

    private void Start()
    {
        LocalizationSettings.SelectedLocale = languages[PlayerPrefs.GetInt("selected_lang", 0)];

        StartCoroutine(WaitToEndScene());
    }

    private IEnumerator WaitToEndScene()
    {
        yield return new WaitForSeconds(delayBeforeNextScene);

        SceneManager.LoadScene(nextScene);
    }
}
