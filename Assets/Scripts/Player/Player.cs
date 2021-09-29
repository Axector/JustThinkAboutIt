using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 10f;
    [SerializeField]
    private float jumpForce = 10f;

    public float PlayerSpeed { get => playerSpeed; }
    public float JumpForce { get => jumpForce; }
}
