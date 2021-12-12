using UnityEngine.UI;
using UnityEngine;

public class ShowPowerUps : DefaultClass
{
    [SerializeField]
    private Image damagePowerUp;
    [SerializeField]
    private Image healthPowerUp;
    [SerializeField]
    private Image livesPowerUp;

    private void Start()
    {
        int powerUpCount = 0;

        if (PlayerPrefs.GetInt("damage_power_up", 0) == 1) {
            InstantiatePowerUp(powerUpCount * -50, damagePowerUp);
            powerUpCount++;
        }

        if (PlayerPrefs.GetInt("health_power_up", 0) == 1) {
            InstantiatePowerUp(powerUpCount * -50, healthPowerUp);
            powerUpCount++;
        }

        if (PlayerPrefs.GetInt("lives_power_up", 0) == 1) {
            InstantiatePowerUp(powerUpCount * -50, livesPowerUp);
        }
    }

    private void InstantiatePowerUp(int positionY, Image powerUpImage)
    {
        Image powerUp = Instantiate(
            powerUpImage,
            gameObject.transform
        );

        powerUp.transform.localPosition = new Vector2(20, positionY - 20);
    }
}
