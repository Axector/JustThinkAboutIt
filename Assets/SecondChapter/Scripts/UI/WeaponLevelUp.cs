using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponLevelUp : DefaultClass
{
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private int[] prices;
    [SerializeField]
    private MenuController menu;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private Image itemImage;

    private Button button;
    private int coins;
    private int level;

    private void Start()
    {
        button = GetComponent<Button>();

        UpdatePlayerStats();
    }

    public void BuyLevelUp()
    {
        if (coins >= prices[level]) {
            // Change coins amout
            menu.SetCoins(prices[level]);

            // Increase weapon level
            PlayerPrefs.SetInt("weapon_level", level + 1);

            UpdatePlayerStats();
        }
        else if (menu.BNeedMoreMoney) {
            menu.BNeedMoreMoney = false;

            StartCoroutine(menu.NeedMoreMoney());
        }
    }

    private void UpdatePlayerStats()
    {
        coins = PlayerPrefs.GetInt("all_money", 0);
        level = PlayerPrefs.GetInt("weapon_level", 0);

        // If level is maximal, disable buy button
        if (level >= prices.Length) {
            button.interactable = false;
            buttonText.text = "Max";
        }
        else {
            buttonText.text = prices[level].ToString();
        }

        itemImage.sprite = sprites[level];
    }
}
