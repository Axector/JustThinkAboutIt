using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : DefaultClass
{
    [SerializeField]
    private Fighter player;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Text healthBartext;

    private void Start()
    {
        CheckHealth();
    }

    public void CheckHealth()
    {
        healthBar.fillAmount = (float)player.healthPoints / player.maxHealthPoints;
        healthBartext.text = player.healthPoints.ToString();
    }
}
