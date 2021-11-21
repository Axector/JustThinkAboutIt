using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.UI;
using UnityEngine;

public class Settings : DefaultClass
{
    [SerializeField]
    private Locale[] languages;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider soundVolumeSlider;
    [SerializeField]
    private Dropdown languageDropDown;
    [SerializeField]
    private GameObject controls;
    [SerializeField]
    private GameObject[] contentToHide;
    [SerializeField]
    private GameObject controlsLayoutParent;
    [SerializeField]
    private GameObject controlsLayout;

    private bool bOpenControls;
    private Music backgroundMusic;

    public bool BOpenControls { get => bOpenControls; }

    private void Awake()
    {
        backgroundMusic = FindObjectOfType<Music>();

        // Set game language
        SetLanguage(PlayerPrefs.GetInt("selected_lang", 0));
        languageDropDown.value = PlayerPrefs.GetInt("selected_lang", 0);

        // Set controls layout
        Instantiate(
            controlsLayout,
            controlsLayoutParent.transform.position,
            Quaternion.identity,
            controlsLayoutParent.transform
        );

        // Set volume for music and sound effects
        musicVolumeSlider.value = PlayerPrefs.GetFloat("music_volume", 1f);
        soundVolumeSlider.value = PlayerPrefs.GetFloat("sound_volume", 1f);
    }

    private void SetLanguage(int index)
    {
        // Select game language
        LocalizationSettings.SelectedLocale = languages[index];
        PlayerPrefs.SetInt("selected_lang", index);
    }

    private void HideContent()
    {
        foreach (GameObject item in contentToHide) {
            item.SetActive(false);
        }
    }

    private void UnhideContent()
    {
        foreach (GameObject item in contentToHide) {
            item.SetActive(true);
        }
    }

    public void ChangeMusicVolume()
    {
        backgroundMusic.SetMusicVolume(musicVolumeSlider.value);
        PlayerPrefs.SetFloat("music_volume", musicVolumeSlider.value);
    }

    public void ChangeSoundVolume()
    {
        PlayerPrefs.SetFloat("sound_volume", soundVolumeSlider.value);
    }

    public void ChangeLanguage()
    {
        SetLanguage(languageDropDown.value);
    }

    public void OpenControls()
    {
        bOpenControls = true;
        controls.SetActive(true);
        HideContent();
    }

    public void CloseControls()
    {
        bOpenControls = false;
        controls.SetActive(false);
        UnhideContent();
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}
