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
    private GameObject secondChapterButtonPlaceholder;
    [SerializeField]
    private Locale[] languages;

    private int bOpenSecondChapter;

    private void Awake()
    {
        // Set game language
        LocalizationSettings.SelectedLocale = languages[PlayerPrefs.GetInt("selected_lang", 0)];
        bOpenSecondChapter = PlayerPrefs.GetInt("open_second_chapter", 0); // 0 - disabled, 1 - to open, 2 - opened

        if (bOpenSecondChapter == 1) { 
            StartCoroutine(ActivateSecondChapter());
        }
        else if (bOpenSecondChapter == 2) {
            EnableSecondChapter();
        }
    }

    private IEnumerator ActivateSecondChapter()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(DestroyButtonPlaceholder());

        // Show Second chapter button
        while (secondChapterButtonPlaceholder) {
            Vector3 rotation = secondChapterButtonPlaceholder.transform.rotation.eulerAngles;
            secondChapterButtonPlaceholder.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z + 1);
            secondChapterButtonPlaceholder.transform.position += (Vector3.down / 3);
            yield return new WaitForSeconds(1/30f);
        }
    }

    private IEnumerator DestroyButtonPlaceholder()
    {
        yield return new WaitForSeconds(3f);

        EnableSecondChapter();
    }

    private void EnableSecondChapter()
    {
        Destroy(secondChapterButtonPlaceholder);
    }

    private IEnumerator DelayBeforeSwithcScene(int index)
    {
        fadingScreenIn.SetActive(true);

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
        StartCoroutine(DelayBeforeSwithcScene(1));
    }

    public void StartSecondChapter()
    {
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
