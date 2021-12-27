using UnityEngine;

public class Player_TopDown : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator animator;
    private RaycastHit2D hit;

    private Vector3 deltaMove;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float xVelocity = Input.GetAxisRaw("Horizontal");
        float yVelocity = Input.GetAxisRaw("Vertical");

        // Reset movement delta
        deltaMove = new Vector3(xVelocity, yVelocity, 0);

        // Player running animation
        animator.SetInteger("velocity", (xVelocity != 0) ? 1 : ((yVelocity != -0) ? 1 : 0));

        // Change sprite look direction if player is moving to the right or left
        if (deltaMove.x > 0) {
            transform.localScale = Vector3.one;
        }
        else if (deltaMove.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Check if player is colliding creature or wall (y axis)
        hit = Physics2D.BoxCast(
            transform.position, 
            boxCollider.size, 
            0, 
            new Vector2(0, deltaMove.y), 
            Mathf.Abs(deltaMove.y * Time.fixedDeltaTime), 
            LayerMask.GetMask("Creature", "Blocker")
        );

        // If player is not colliding anything (y axis)
        if (hit.collider == null) {
            // Vertical movement
            transform.Translate(0, deltaMove.y * Time.deltaTime, 0);
        }

        // Check if player is colliding creature or wall (x axis)
        hit = Physics2D.BoxCast(
            transform.position, 
            boxCollider.size, 
            0, 
            new Vector2(deltaMove.x, 0), 
            Mathf.Abs(deltaMove.x * Time.fixedDeltaTime), 
            LayerMask.GetMask("Creature", "Blocker")
        );

        // If player is not colliding anything (x axis)
        if (hit.collider == null) {
            // Horizontal movement
            transform.Translate(deltaMove.x * Time.deltaTime, 0, 0);
        }
    }
}
