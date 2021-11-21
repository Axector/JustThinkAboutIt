using UnityEngine.SceneManagement;
using UnityEngine;

public class QuickMenu : DefaultClass
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private Settings settingsMenu;

    public GameObject Menu { get => menu; }
    public Settings SettingsMenu { get => settingsMenu; }

    public void Exit()
    {
        // Load main menu scene
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        // Open settings and close menu
        settingsMenu.gameObject.SetActive(true);
        menu.SetActive(false);
    }

    public void Resume()
    {
        // Resume game and close menu
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void CloseSettings()
    {
        // Close settings and open menu
        if (settingsMenu.gameObject.activeSelf) {
            settingsMenu.gameObject.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void BackFromControls()
    {
        settingsMenu.CloseControls();
    }
}
