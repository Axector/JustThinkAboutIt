using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : DefaultClass
{
    [SerializeField]
    private Fighter player;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Text healthBarText;
    [SerializeField]
    private Color greatColor;
    [SerializeField]
    private Color goodColor;
    [SerializeField]
    private Color badColor;

    private void Start()
    {
        CheckHealth();
    }

    public void CheckHealth()
    {
        healthBar.fillAmount = (float)player.healthPoints / player.maxHealthPoints;
        healthBarText.text = player.healthPoints.ToString();


        if (player.healthPoints < player.maxHealthPoints / 3) {
            healthBar.color = badColor;
        }
        else if (player.healthPoints < player.maxHealthPoints * 2 / 3) {
            healthBar.color = goodColor;
        }
        else {
            healthBar.color = greatColor;
        }
    }
}
