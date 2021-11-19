using UnityEngine;

public class ShakingObject : DefaultClass
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector2 angle;

    private float maxAngle;
    private float minAngle;
    private float defaultAngle;

    private void Start()
    {
        maxAngle = angle.y;
        minAngle = angle.x;

        // Get default angle to start with
        defaultAngle = Mathf.Max(Mathf.Abs(minAngle), Mathf.Abs(maxAngle));
    }

    private void FixedUpdate()
    {
        // Get new rotation using sine wave
        float newRotation = defaultAngle * Mathf.Sin(speed * Time.fixedTime);

        // Check if rotation angle is inside min and max range
        newRotation = (newRotation < minAngle) ? minAngle : 
                      (newRotation > maxAngle) ? maxAngle : newRotation;

        // Set new rotation
        transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }
}
