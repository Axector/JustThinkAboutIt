using UnityEngine;

public class Enemy_Mine : Enemy_Ground_Shooter
{
    [SerializeField]
    private GameObject pivot;

    private float rotationMax = 40f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // TODO: Rotate eye to player
    }
}
