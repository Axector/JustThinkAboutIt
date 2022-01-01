using UnityEngine.UI;
using UnityEngine;

public class ShowPowerUps : DefaultClass
{
    [SerializeField]
    private Image damagePowerUp;
    [SerializeField]
    protected Image healthPowerUp;
    [SerializeField]
    protected Image livesPowerUp;

    private void Start()
    {
        int powerUpCount = 0;

        // Show damage power-up
        if (PlayerPrefs.GetInt("damage_power_up", 0) == 1) {
            InstantiatePowerUp(powerUpCount * -50, damagePowerUp);
            powerUpCount++;
        }

        // Show health power-up
        if (PlayerPrefs.GetInt("health_power_up", 0) == 1) {
            InstantiatePowerUp(powerUpCount * -50, healthPowerUp);
            powerUpCount++;
        }

        // Show second live power-up
        if (PlayerPrefs.GetInt("lives_power_up", 0) == 1) {
            InstantiatePowerUp(powerUpCount * -50, livesPowerUp);
        }
    }

    protected void InstantiatePowerUp(int positionY, Image powerUpImage)
    {
        float offset = 20f;

        // Instatiate power-up sprite
        Image powerUp = Instantiate(
            powerUpImage,
            gameObject.transform
        );

        powerUp.transform.localPosition = new Vector2(offset, positionY - offset);
    }
}
