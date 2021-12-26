using System.Collections;
using UnityEngine;
using System;

public class GameTime : DefaultClass
{
    [SerializeField]
    private bool bTimeCountPoint;

    private void Awake()
    {
        // Set enter date time
        if (bTimeCountPoint) {
            PlayerPrefs.SetString("current_date", DateTime.Now.ToString());
        }
        else {
            SetTotalTime();
        }
    }

    private void SetTotalTime()
    {
        // Get time from enter to exit
        DateTime date = DateTime.Parse(PlayerPrefs.GetString("current_date"));
        TimeSpan deltaTimePlayed = DateTime.Now - date;

        // Store time spent on playing the game
        if (PlayerPrefs.HasKey("total_time")) {
            TimeSpan lastTime = TimeSpan.Parse(PlayerPrefs.GetString("total_time"));

            PlayerPrefs.SetString("total_time", GetTimeString(lastTime + deltaTimePlayed));
        }
        else {
            PlayerPrefs.SetString("total_time", GetTimeString(deltaTimePlayed));
        }
    }
}
