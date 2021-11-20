using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : DefaultClass
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Text healthPointsText;
    [SerializeField]
    private Image healthPointsBar;
    [SerializeField]
    private float rayDistance = 1f;
    [SerializeField]
    private float rayOffsetRight = 1f;
    [SerializeField]
    private float rayOffsetLeft = 1f;
    
    private Rigidbody2D pRigidBody2D;   // prefix p means player
    private SpriteRenderer pSpriteRenderer;
    private Transform pTransform;
    private Animator pAnimator;

    private float velocityX = 0;
    private float movementSpeed;
    private float jumpForce;
    private bool isGrounded         = false;
    private bool canAttackDown      = false;
    private bool isLeftAttacking    = false;
    private bool isRightAttacking   = false;
    private bool isWaitingForAttack = false;

    private void Start()
    {
        // Get different components of a player
        pRigidBody2D = player.GetComponent<Rigidbody2D>();
        pSpriteRenderer = player.GetComponent<SpriteRenderer>();
        pTransform = player.GetComponent<Transform>();
        pAnimator = player.GetComponent<Animator>();

        // Set basic stats for player movement
        movementSpeed = player.PlayerSpeed;
        jumpForce = player.JumpForce;
    }

    private void Update()
    {
        // Get velocity value from axis
        velocityX = Input.GetAxis("Horizontal");

        bool playerIsCutscene = player.IsCutscene;

        // Checks if player is grounded
        SetIsGrounded();

        // Player can attack down only once if not grounded
        if (isGrounded && !playerIsCutscene) {
            canAttackDown = true;
        }

        // Jump when Space button is pressed and the player is grounded and alive
        if (
            Input.GetButtonDown("Jump") && 
            isGrounded && 
            player.IsAlive && 
            !playerIsCutscene
        ) {
            Jump(jumpForce);
        }

        // Player attack if is alive
        if (player.IsAlive && !playerIsCutscene) {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        // Movement to left and right while player is alive and it is not cutscene
        if (player.IsAlive && !player.IsCutscene) {
            Movement();

            // Set parameter for animator to animate running
            pAnimator.SetFloat("velocity", Mathf.Abs(velocityX));
        }
        // Move to the right direction
        else if (player.IsAlive) {
            pTransform.position += new Vector3(movementSpeed * Time.fixedDeltaTime, 0, 0);
            pSpriteRenderer.flipX = false;
        }
    }

    private void Attack()
    {
        HorizontalAttack();

        // PLayer attack up
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isWaitingForAttack) {
            isWaitingForAttack = true;
            pAnimator.Play("Player_Attack_Up");


            StartCoroutine(WaitForAttack());
        }

        // PLayer attack down
        if (Input.GetKeyDown(KeyCode.DownArrow) && canAttackDown) {
            pAnimator.Play("Player_Flip");
            canAttackDown = false;

            // Attack down needs a leap
            float jumpForceValue = jumpForce / 2;

            // Jump forward
            Jump(jumpForceValue);
            pRigidBody2D.AddForce(
                (pSpriteRenderer.flipX ? Vector2.left : Vector2.right) * jumpForceValue / 2,
                ForceMode2D.Impulse
            );
        }
    }

    private void HorizontalAttack()
    {
        // PLayer attack left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            isLeftAttacking = true;
        }

        // Stop to attack left
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            isLeftAttacking = false;
        }

        // PLayer attack right
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            isRightAttacking = true;
        }

        // Stop to attack right
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            isRightAttacking = false;
        }

        // PLayer long horizontal attack
        pAnimator.SetBool(
            "longAttack",
            Input.GetKey(KeyCode.RightArrow) || 
            Input.GetKey(KeyCode.LeftArrow)
        );

        // Play specific animaion if player is attacking
        if (isLeftAttacking || isRightAttacking) {
            pAnimator.Play((velocityX != 0) ? "Player_RunAttack" : "Player_Attack_2");
        }
    }

    private void SetIsGrounded()
    {
        // Get Floor layer
        LayerMask mask = LayerMask.GetMask("Floor");

        // DEBUG
        Debug.DrawRay(new Vector2(pTransform.position.x + rayOffsetRight, pTransform.position.y), Vector2.down * rayDistance, Color.red);
        Debug.DrawRay(new Vector2(pTransform.position.x - rayOffsetLeft, pTransform.position.y), Vector2.down * rayDistance, Color.red);

        // Cast two rays down to check if the player is on the ground
        RaycastHit2D[] rightHit = Physics2D.RaycastAll(
                new Vector2(pTransform.position.x + rayOffsetRight, pTransform.position.y), 
                Vector2.down, 
                rayDistance, 
                mask
            );
        RaycastHit2D[] leftHit = Physics2D.RaycastAll(
                new Vector2(pTransform.position.x - rayOffsetLeft, pTransform.position.y), 
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
        if (velocityX < 0 && !isRightAttacking || isLeftAttacking) {
            pSpriteRenderer.flipX = true;
        }
        else if (velocityX > 0 && !isLeftAttacking || isRightAttacking) {
            pSpriteRenderer.flipX = false;
        }
    }

    public void ShowHealth(Player player = null)
    {
        // If an object was not passed
        if (!player) {
            player = this.player;
        }

        // Get player health value
        int playerHealth = player.Health;

        // Set health points text
        healthPointsText.text = playerHealth.ToString();

        // Get health percentage
        float healthPercentage = (float)playerHealth / player.MaxHealth;
        healthPointsBar.fillAmount = healthPercentage;

        // Change color of the helth points depending on percentage
        if (healthPercentage >= .66f) {
            healthPointsBar.color = successColor;
        }
        else if (healthPercentage >= .33f) {
            healthPointsBar.color = warningColor;
        }
        else {
            healthPointsBar.color = dangerColor;
        }
    }

    private void Jump(float jumpForceValue)
    {
        pRigidBody2D.AddForce(Vector2.up * jumpForceValue, ForceMode2D.Impulse);
    }

    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(0.2f);

        isWaitingForAttack = false;
    }
}