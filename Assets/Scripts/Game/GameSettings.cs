using UnityEngine;

public static class GameSettings
{
    // Colors
    public static Color successColor = new Color(0, 120 / 255f, 0, 120 / 255f);     // Green
    public static Color warningColor = new Color(1, 150 / 255f, 0, 120 / 255f);     // Yellow
    public static Color dangerColor  = new Color(150 / 255f, 0, 0, 120 / 255f);     // Red

    // Settings
    public static float soundVolume = PlayerPrefs.GetFloat("soundVolume", 1f);
}
