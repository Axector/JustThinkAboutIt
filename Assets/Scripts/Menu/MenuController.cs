using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class MenuController : DefaultClass
{
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject fadingScreenIn;
    [SerializeField]
    private Locale[] languages;

    private void Awake()
    {
        // Set game language
        LocalizationSettings.SelectedLocale = languages[PlayerPrefs.GetInt("selected_lang", 0)];
    }

    private IEnumerator DelayBeforeSwithcScene(int index)
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(index);
    }

    private IEnumerator DelayBeforeExit()
    {
        yield return new WaitForSeconds(4f);

        Application.Quit();

        // DEBUG
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void StartFirstChapter()
    {
        fadingScreenIn.SetActive(true);

        StartCoroutine(DelayBeforeSwithcScene(1));
    }

    public void StartSecondChapter()
    {
        fadingScreenIn.SetActive(true);

        StartCoroutine(DelayBeforeSwithcScene(0));
    }

    public void EnterSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void ExitSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        fadingScreenIn.SetActive(true);

        StartCoroutine(DelayBeforeExit());
    }
}
