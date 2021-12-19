using UnityEngine;
using System;

public class DefaultClass : MonoBehaviour
{
    /// Variables
    // Colors
    protected Color successColor   = new Color(0, 120 / 255f, 0, 120 / 255f);        // Green
    protected Color warningColor   = new Color(1, 150 / 255f, 0, 120 / 255f);        // Yellow
    protected Color dangerColor    = new Color(150 / 255f, 0, 0, 120 / 255f);        // Red
    protected Color lightGreyColor = new Color(188 / 255f, 188 / 255f, 188 / 255f);  // Light Grey

    /// Functions
    protected bool NearlyEqual(float a, float b, float delta)
    {
        return Mathf.Abs(a - b) <= delta;
    }

    protected bool NearlyEqual(Vector2 a, Vector2 b, float delta)
    {
        return Vector2.Distance(a, b) <= delta;
    }

    protected bool NearlyEqual(Vector3 a, Vector3 b, float delta)
    {
        return Vector3.Distance(a, b) <= delta;
    }

    protected float NormalizeRotationAngle(float angle)
    {
        if (angle < 0) {
            angle %= 360;

            return angle + 360;
        }

        return angle % 360f;
    }

    protected Quaternion GetLookAtRotation(Vector3 direction)
    {
        // Set enemy rotation to face the player
        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    protected void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.volume = PlayerPrefs.GetFloat("sound_volume", 1f);
        source.Play();
    }

    protected bool Chance(float percent)
    {
        // Get percent as decimal
        percent /= 100;

        return percent > UnityEngine.Random.Range(0, 1f);
    }

    protected string GetTimeString(TimeSpan deltaTimePlayed)
    {
        return new TimeSpan(
            deltaTimePlayed.Hours,
            deltaTimePlayed.Minutes,
            deltaTimePlayed.Seconds
        ).ToString();
    }
}
