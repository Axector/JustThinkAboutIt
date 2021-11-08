using UnityEngine;

public static class GameSettings
{
    public static float soundVolume = PlayerPrefs.GetFloat("soundVolume", 1f);
}
