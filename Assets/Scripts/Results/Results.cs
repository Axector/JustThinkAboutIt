using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Results : DefaultClass
{
    [SerializeField]
    private Image healthPointsBar;
    [SerializeField]
    private Text healthPointsText;
    [SerializeField]
    private Text currentMoneyText;
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

    private int playerHealth;
    private int playerMaxHealth;
    private int currentPlayerMoney;

    private void Awake()
    {
        playerMaxHealth = PlayerPrefs.GetInt("player_max_health", 0);
        playerHealth = PlayerPrefs.GetInt("player_health", playerMaxHealth);
        currentPlayerMoney = PlayerPrefs.GetInt("player_money", 0);

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

            yield return new WaitForSeconds(delayBetweenResults);
        }
    }

    private IEnumerator ShowHealthPoints()
    {
        for (int currentHealth = playerMaxHealth; currentHealth > playerHealth; currentHealth--) {
            CheckHealthColor(currentHealth);

            yield return new WaitForSeconds(1f / valueChangeSpeed);
        }
    }

    private IEnumerator ShowCurrentMoney()
    {
        for (int currentMoney = 0; currentMoney <= currentPlayerMoney; currentMoney++) {
            currentMoneyText.text = currentMoney.ToString();

            yield return new WaitForSeconds(1f / valueChangeSpeed);
        }

        coinIcon.SetActive(true);
    }

    public void Exit()
    {
        // Enable fading screen
        fadingScreen.SetActive(true);

        // Save earned money
        int newMoney = PlayerPrefs.GetInt("all_money", 0) + currentPlayerMoney;
        PlayerPrefs.SetInt("all_money", newMoney);

        // DEBUG
        Debug.Log(PlayerPrefs.GetInt("all_money", 0));
        PlayerPrefs.SetInt("all_money", 0);

        // Return to menu
        StartCoroutine(DelayBeforeSwithcScene(0));
    }

    public void Continue()
    {
        // Enable fading screen
        fadingScreen.SetActive(true);

        // Return to menu
        StartCoroutine(DelayBeforeSwithcScene(PlayerPrefs.GetInt("next_level", 0)));
    }

    private IEnumerator DelayBeforeSwithcScene(int index)
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(index);
    }
}
