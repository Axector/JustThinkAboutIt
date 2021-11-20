using UnityEngine;

public class DefaultClass : MonoBehaviour
{
    /// Variables
    // Colors
    public static Color successColor   = new Color(0, 120 / 255f, 0, 120 / 255f);        // Green
    public static Color warningColor   = new Color(1, 150 / 255f, 0, 120 / 255f);        // Yellow
    public static Color dangerColor    = new Color(150 / 255f, 0, 0, 120 / 255f);        // Red
    public static Color darkGreyColor  = new Color(90 / 255f, 90 / 255f, 90 / 255f);     // Dark Grey
    public static Color lightGreyColor = new Color(188 / 255f, 188 / 255f, 188 / 255f);  // Light Grey

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
        source.Play();
    }
}
