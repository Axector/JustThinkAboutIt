using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class MenuController : DefaultClass
{
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private GameObject fadingScreenIn;
    [SerializeField]
    private GameObject secondChapterButtonPlaceholder;
    [SerializeField]
    private Text shopCoins;

    private int bOpenSecondChapter;
    private int coins;
    private bool bNeedMoreMoney = true;

    private void Awake()
    {
        // DEBUG
        PlayerPrefs.SetInt("all_money", 200);

        bOpenSecondChapter = PlayerPrefs.GetInt("open_second_chapter", 0); // 0 - disabled, 1 - to open, 2 - opened

        // Set coins number to Shop
        coins = PlayerPrefs.GetInt("all_money", 0);
        SetCoinsShop();

        // Open second chapter button
        if (bOpenSecondChapter == 1) {
            StartCoroutine(ActivateSecondChapter());
        }
        else if (bOpenSecondChapter == 2) {
            EnableSecondChapter();
        }
    }

    private void SetCoinsShop()
    {
        int coinsDigitCount = (int)Math.Floor(Math.Log10(coins)) + 1;
        shopCoins.GetComponent<RectTransform>().sizeDelta = new Vector2(26 * coinsDigitCount, 0);
        shopCoins.text = coins.ToString();
    }

    private IEnumerator ActivateSecondChapter()
    {
        PlayerPrefs.SetInt("open_second_chapter", 2);

        yield return new WaitForSeconds(1f);

        StartCoroutine(DestroyButtonPlaceholder());

        // Show Second chapter button
        while (secondChapterButtonPlaceholder) {
            Vector3 rotation = secondChapterButtonPlaceholder.transform.rotation.eulerAngles;
            secondChapterButtonPlaceholder.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z + 1);
            secondChapterButtonPlaceholder.transform.position += (Vector3.down / 3);
            yield return new WaitForSeconds(1 / 30f);
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

        SceneManager.LoadScene(13);
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

    public void EnterShop()
    {
        shopMenu.SetActive(true);
    }

    public void ExitShop()
    {
        shopMenu.SetActive(false);
    }

    public void ExitGame()
    {
        fadingScreenIn.SetActive(true);

        StartCoroutine(DelayBeforeExit());
    }

    public void BuyPowerUp(ShopButton powerUpIndex)
    {
        if (coins >= powerUpIndex.Cost) {
            PlayerPrefs.SetInt(powerUpIndex.Code, 1);
            powerUpIndex.button.interactable = false;
            SetCoins(powerUpIndex.Cost);
        }
        else if (bNeedMoreMoney) {
            bNeedMoreMoney = false;

            StartCoroutine(NeedMoreMoney());
        }
    }

    private void SetCoins(int lost)
    {
        coins -= lost;
        PlayerPrefs.SetInt("all_money", coins);
        SetCoinsShop();
    }

    private IEnumerator NeedMoreMoney()
    {
        Color coinsColor = shopCoins.color;

        for (int i = 0; i < 3; i++) {
            shopCoins.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            shopCoins.color = coinsColor;

            yield return new WaitForSeconds(0.1f);
        }

        bNeedMoreMoney = true;
    }
}