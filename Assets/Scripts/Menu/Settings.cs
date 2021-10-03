using UnityEngine;

public class Settings
{
    private static float CameraOrthographicSize = PlayerPrefs.GetFloat("CameraOrthographicSize", 7f);

    public static float getCameraOrthographicSize()
    {
        return CameraOrthographicSize;
    }

    public static void setCameraOrthographicSize(float value)
    {
        CameraOrthographicSize = value;
        PlayerPrefs.SetFloat("CameraOrthographicSize", value);
    }
}
