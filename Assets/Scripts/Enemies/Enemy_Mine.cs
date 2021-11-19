using UnityEngine;

public class Enemy_Mine : Enemy_Ground_Shooter
{
    [SerializeField]
    private GameObject pivot;
    [SerializeField]
    private float rotationMax;
    [SerializeField]
    private float rotationOffset;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (seePlayer) {
            // Get rotation angle to look at player
            Vector3 directionToPlayer = player.transform.position - pivot.transform.position;
            float rotationAngle = GetLookAtRotation(directionToPlayer).eulerAngles.z;
            rotationAngle = NormalizeRotationAngle(rotationAngle);

            // Include offset that depends on the object rotation
            rotationAngle -= rotationOffset;

            // DEBUG
            Debug.Log(rotationAngle);

            // Check angle maximum and minimum
            rotationAngle = rotationAngle < rotationMax ? rotationAngle : rotationMax;
            rotationAngle = rotationAngle > -rotationMax ? rotationAngle : -rotationMax;

            // Rotate to player
            pivot.transform.localRotation = Quaternion.Euler(
                0,
                0,
                rotationAngle
            );
        }
    }
}
