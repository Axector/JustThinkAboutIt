using UnityEngine;

public class CutscenePlayer : DefaultClass
{
    [SerializeField]
    private float playerSpeed = 10f;

    private bool isAlive = true;
    private Animator animator;

    public float PlayerSpeed { get => playerSpeed; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }

    private void Awake()
    {
        // Get animator
        animator = GetComponent<Animator>();

        animator.SetBool("isAlive", isAlive);
        animator.SetBool("isGrounded", true);
    }
}
