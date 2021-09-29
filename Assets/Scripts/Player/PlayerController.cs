using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerGameObject;

    [SerializeField]
    private float rayDistance = 1f;
    [SerializeField]
    private float rayOffsetX = 1f;

    private float velocityX = 0;
    private float movementSpeed;
    private float jumpForce;
    private bool isGrounded = false;

    private Player player;
    private Rigidbody2D pRigidBody2D;   // prefix p means player
    private SpriteRenderer pSpriteRenderer;
    private Transform pTransform;

    private void Start()
    {
        player = playerGameObject.GetComponent<Player>();
        pRigidBody2D = playerGameObject.GetComponent<Rigidbody2D>();
        pSpriteRenderer = playerGameObject.GetComponent<SpriteRenderer>();
        pTransform = playerGameObject.GetComponent<Transform>();

        movementSpeed = player.PlayerSpeed;
        jumpForce = player.JumpForce;
    }

    private void Update()
    {
        // Get velocity value from axis
        velocityX = Input.GetAxis("Horizontal");

        // Checks if player is grounded
        setIsGrounded();

        // Jump when Space button is pressed
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            pRigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Movement to left and right
        Movement();
    }

    private void setIsGrounded()
    {
        // Get Floor layer
        LayerMask mask = LayerMask.GetMask("Floor");

        // Cast two rays down to check if the player is on the ground
        RaycastHit2D[] rightHit = Physics2D.RaycastAll(
                new Vector2(pTransform.position.x + rayOffsetX, pTransform.position.y), 
                Vector2.down, 
                rayDistance, 
                mask
            );
        RaycastHit2D[] leftHit = Physics2D.RaycastAll(
                new Vector2(pTransform.position.x - rayOffsetX, pTransform.position.y), 
                Vector2.down, 
                rayDistance, 
                mask
            );
        
        // Change isGrounded value if ray hit any Floor
        isGrounded = leftHit.Length > 0 || rightHit.Length > 0;
    }

    void Movement()
    {
        pTransform.position += new Vector3(velocityX * movementSpeed * Time.deltaTime, 0, 0);

        // Player sprite rotation (left/right)
        // When velocity == 0, should stay in last rotation
        if (velocityX < 0) {
            pSpriteRenderer.flipX = true;
        }
        else if (velocityX > 0) {
            pSpriteRenderer.flipX = false;
        }
    }
}