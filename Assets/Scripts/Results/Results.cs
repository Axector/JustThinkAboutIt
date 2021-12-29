using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Results : DefaultClass
{
    [SerializeField]
    private Image healthPointsBar;
    [SerializeField]
    private Text healthPointsText;
    [SerializeField]
    private Text currentMoneyText;
    [SerializeField]
    private Text currentTimeText;
    [SerializeField]
    private float delayBetweenResults;
    [SerializeField]
    private float valueChangeSpeed;
    [SerializeField]
    private GameObject coinIcon;
    [SerializeField]
    private GameObject fadingScreen;
    [SerializeField]
    private GameObject[] children;
    [SerializeField]
    private GameObject buttons;
    [SerializeField]
    private GameObject buttonsExitOnly;

    private int playerHealth;
    private int playerMaxHealth;
    private int currentPlayerMoney;
    private int currentRunMoney;
    private int bSaveMoney;     // 1 - true, 0 - false

    private void Awake()
    {
        playerMaxHealth = PlayerPrefs.GetInt("player_max_health", 0);
        playerHealth = PlayerPrefs.GetInt("player_health", playerMaxHealth);
        currentPlayerMoney = PlayerPrefs.GetInt("player_money", 0);
        currentRunMoney = PlayerPrefs.GetInt("player_run_money", 0);
        bSaveMoney = PlayerPrefs.GetInt("save_money", 0);
        PlayerPrefs.SetInt("save_money", 0);

        if (bSaveMoney == 1) {
            currentRunMoney += currentPlayerMoney;
        }

        StartCoroutine(ShowResults());
    }

    private void CheckHealthColor(int currentHealth)
    {
        // Show max health / current health
        healthPointsText.text = currentHealth.ToString() + "/" + playerMaxHealth.ToString();

        // Get health percentage
        float healthPercentage = (float)currentHealth / playerMaxHealth;
        healthPointsBar.fillAmount = healthPercentage;

        // Change color of the helth points depending on percentage
        if (healthPercentage >= .66f) {
            healthPointsBar.color = successColor;
            healthPointsText.color = successColor;
        }
        else if (healthPercentage >= .33f) {
            healthPointsBar.color = warningColor;
            healthPointsText.color = warningColor;
        }
        else {
            healthPointsBar.color = dangerColor;
            healthPointsText.color = dangerColor;
        }
    }

    private IEnumerator ShowResults()
    {
        foreach (GameObject child in children) {
            child.SetActive(true);

            // Start health animation
            if (child == healthPointsBar.gameObject) {
                StartCoroutine(ShowHealthPoints());
            }

            // Start money animation
            if (child == currentMoneyText.gameObject) { 
                StartCoroutine(ShowCurrentMoney());
            }

            // Show current time spent
            if (child == currentTimeText.gameObject) {
                TimeSpan time = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("current_date"));
                currentTimeText.text = GetTimeString(time);
            }

            yield return new WaitForSeconds(delayBetweenResults);
        }

        int nextLevel = PlayerPrefs.GetInt("next_level", 1);

        if (nextLevel == 1 || nextLevel == 12) {
            buttonsExitOnly.SetActive(true);
        }
        else {
            buttons.SetActive(true);
        }
    }

    private IEnumerator ShowHealthPoints()
    {
        for (int currentHealth = playerMaxHealth; currentHealth >= playerHealth; currentHealth--) {
            CheckHealthColor(currentHealth);

            yield return new WaitForSeconds(1f / valueChangeSpeed);
        }
    }

    private IEnumerator ShowCurrentMoney()
    {
        for (int currentMoney = 0; currentMoney <= currentRunMoney; currentMoney++) {
            currentMoneyText.text = currentMoney.ToString();

            yield return new WaitForSeconds(1f / (valueChangeSpeed * 2));
        }

        coinIcon.SetActive(true);
    }

    public void Exit()
    {
        // Enable fading screen
        fadingScreen.SetActive(true);

        // Save earned money
        int newMoney = PlayerPrefs.GetInt("all_money", 0) + currentRunMoney;
        PlayerPrefs.SetInt("all_money", newMoney);

        // Reset player info
        PlayerPrefs.SetInt("player_money", 0);
        PlayerPrefs.SetInt("player_run_money", 0);
        PlayerPrefs.SetInt("player_health", playerMaxHealth);

        // Reset power-ups
        PlayerPrefs.SetInt("damage_power_up", 0);
        PlayerPrefs.SetInt("health_power_up", 0);
        PlayerPrefs.SetInt("lives_power_up", 0);

        // Return to menu or titles
        if (PlayerPrefs.GetInt("next_level", 0) == 12) {
            StartCoroutine(DelayBeforeSwithcScene(12));
        }
        else {
            StartCoroutine(DelayBeforeSwithcScene(1));
        }

    }

    public void Continue()
    {
        // Enable fading screen
        fadingScreen.SetActive(true);

        PlayerPrefs.SetInt("player_run_money", currentRunMoney);
        PlayerPrefs.SetInt("player_money", 0);

        // Return to menu
        StartCoroutine(DelayBeforeSwithcScene(PlayerPrefs.GetInt("next_level", 0)));
    }

    private IEnumerator DelayBeforeSwithcScene(int index)
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(index);
    }
}
