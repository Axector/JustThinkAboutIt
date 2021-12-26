using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class QuickMenu : DefaultClass
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private Settings settingsMenu;
    [SerializeField]
    private GameObject fadingScreen;
    [SerializeField]
    private Text shopCoins;

    public GameObject Menu { get => menu; }
    public Settings SettingsMenu { get => settingsMenu; }

    public void Exit()
    {
        // Load main menu scene
        StartCoroutine(ToExit());
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
        SetCoinsShop();
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

    private IEnumerator ToExit()
    {
        fadingScreen.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        // Reset power-ups
        PlayerPrefs.SetInt("damage_power_up", 0);
        PlayerPrefs.SetInt("health_power_up", 0);
        PlayerPrefs.SetInt("lives_power_up", 0);
        PlayerPrefs.SetInt("player_money", 0);
        PlayerPrefs.SetInt("player_run_money", 0);

        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    private void SetCoinsShop()
    {
        int coins = PlayerPrefs.GetInt("player_run_money", 0) + PlayerPrefs.GetInt("player_money", 0);
        int coinsDigitCount = (int)Math.Floor(Math.Log10(coins)) + 1;
        shopCoins.GetComponent<RectTransform>().sizeDelta = new Vector2(26 * coinsDigitCount, 50);
        shopCoins.text = coins.ToString();
    }
}
