using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShowPowerUps_TopDown : ShowPowerUps
{
    private void Start()
    {
        int powerUpCount = 0;

        // Show health power-up
        if (PlayerPrefs.GetInt("health_power_up_2", 0) == 1) {
            InstantiatePowerUp(powerUpCount * -50, healthPowerUp);
            powerUpCount++;
        }

        // Show second live power-up
        if (PlayerPrefs.GetInt("lives_power_up_2", 0) == 1) {
            InstantiatePowerUp(powerUpCount * -50, livesPowerUp);
        }
    }
}
