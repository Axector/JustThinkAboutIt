using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerGameObject;
    public Text healthPointsText;

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
    private Animator pAnimator;

    private CameraController cameraController;

    private void Start()
    {
        // Get different components of a player
        player = playerGameObject.GetComponent<Player>();
        pRigidBody2D = playerGameObject.GetComponent<Rigidbody2D>();
        pSpriteRenderer = playerGameObject.GetComponent<SpriteRenderer>();
        pTransform = playerGameObject.GetComponent<Transform>();
        pAnimator = playerGameObject.GetComponent<Animator>();

        // Set basic stats for player movement
        movementSpeed = player.PlayerSpeed;
        jumpForce = player.JumpForce;

        // Get camera controller
        cameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        // Get velocity value from axis
        velocityX = Input.GetAxis("Horizontal");

        // Checks if player is grounded
        setIsGrounded();

        // Jump when Space button is pressed and the player is grounded and alive
        if (Input.GetButtonDown("Jump") && isGrounded && player.IsAlive)
        {
            pRigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    cameraController.selectNextCamera();
        //}
    }

    private void FixedUpdate()
    {
        // Movement to left and right while player is alive
        if (player.IsAlive) {
            Movement();

            // Set parameter for animator to animate running
            pAnimator.SetFloat("velocity", Mathf.Abs(velocityX));
        }
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

        // Set parameter for animator to animate jumping
        pAnimator.SetBool("isGrounded", isGrounded);
    }

    private void Movement()
    {
        pTransform.position += new Vector3(velocityX * movementSpeed * Time.fixedDeltaTime, 0, 0);

        // Player sprite rotation (left/right)
        // When velocity == 0, should stay in last rotation
        if (velocityX < 0) {
            pSpriteRenderer.flipX = true;
        }
        else if (velocityX > 0) {
            pSpriteRenderer.flipX = false;
        }
    }

    public void showHealth()
    {
        int playerHealth = player.Health;

        // Set health points text
        healthPointsText.text = playerHealth.ToString();

        // Get health percentage
        float healthPercentage = (float)playerHealth / player.MaxHealth;

        // Change color of the helth points depending on percentage
        if (healthPercentage >= .66f) {
            healthPointsText.color = Color.green;
        }
        else if (healthPercentage >= .33f) {
            healthPointsText.color = Color.yellow;
        }
        else {
            healthPointsText.color = Color.red;
        }
    }
}