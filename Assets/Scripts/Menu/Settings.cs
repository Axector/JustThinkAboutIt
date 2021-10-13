using UnityEngine;

public class Settings
{
    private static float CameraOrthographicSize = PlayerPrefs.GetFloat("CameraOrthographicSize", 7f);

    public static float GetCameraOrthographicSize()
    {
        return CameraOrthographicSize;
    }

    public static void SetCameraOrthographicSize(float value)
    {
        CameraOrthographicSize = value;
        PlayerPrefs.SetFloat("CameraOrthographicSize", value);
    }
}
